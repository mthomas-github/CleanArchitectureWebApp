using System.Data;
using System.Globalization;
using Dapper;
using DocuSign.eSign.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;
using ThirdPartyFreight.Application.Abstractions.Caching;
using ThirdPartyFreight.Application.Abstractions.Clock;
using ThirdPartyFreight.Application.Abstractions.Data;
using ThirdPartyFreight.Application.Abstractions.DocuSign;
using ThirdPartyFreight.Application.Shared;
using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.Envelopes;
using Envelope = ThirdPartyFreight.Domain.Envelopes.Envelope;


namespace ThirdPartyFreight.Infrastructure.DocuSign.BackgroundJobs;

[DisallowConcurrentExecution]
internal sealed class ProcessStatusUpdateJob(
    ISqlConnectionFactory sqlConnectionFactory,
    IOptions<DocuSignOptions> docuSignOptions,
    ILogger<ProcessStatusUpdateJob> logger,
    IDocuSignService docuSignService,
    ICacheService cacheService,
    IEnvelopeRepository  envelopeRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider) : IJob
{

    private readonly DocuSignOptions _docuSignOptions = docuSignOptions.Value;


    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("Processing Envelope Status");

        using IDbConnection connection = sqlConnectionFactory.CreateConnection();
        using IDbTransaction transaction = connection.BeginTransaction();
        string? lastQueryDateTime = await cacheService.GetAsync<string>("LastQueryDateTime");
        if (lastQueryDateTime == null)
        {
            DateTime currentTime = dateTimeProvider.UtcNow.AddDays(-1);
            lastQueryDateTime = currentTime.ToString("s");
            await cacheService.SetAsync("LastQueryDateTime", lastQueryDateTime,
                TimeSpan.FromHours(_docuSignOptions.ExpireInHoursCache));
        }

        //Get List Of Envelopes, that need to check envelopeStatus
        IReadOnlyList<EnvelopeResponse> processingEnvelopeIds =
            await GetProcessingEnvelopeIdsAsync(connection, transaction);

        if (processingEnvelopeIds.Count != 0)
        {
            // Call DocuSign To Get Updates
            var request = new StatusRequest(
                UserFilter: "sender",
                EnvelopeIds: string.Join(",", processingEnvelopeIds.Select(e => e.EnvelopeId)),
                IncludeInformation: "custom_fields,recipients",
                OrderBy: "status_changed",
                FromStatus: "Changed",
                StatusToInclude: "completed,declined,delivered,signed,timedout",
                FromDate: lastQueryDateTime,
                ToDate: dateTimeProvider.UtcNow.ToString("s"));

            EnvelopesInformation response = await docuSignService.GetEnvelopesInformation(request);
            string resultSize = response.ResultSetSize;
            
            if (int.Parse(resultSize, CultureInfo.InvariantCulture) > 0)
            {
                if (response.Envelopes is null)
                {
                    logger.LogWarning("Got Null Response From DocuSign no Envelopes returned");
                    return;
                }
                
                IEnumerable<UpdatedEnvelope> updatedEnvelope = response.Envelopes.Select(envelope =>
                {
                    EnvelopeResponse? envDbRecord =
                        processingEnvelopeIds.FirstOrDefault(e => e.EnvelopeId.ToString() == envelope.EnvelopeId);
                    
                    if (envDbRecord is null)
                    {
                        logger.LogWarning("Unable To Pull Envelope Record For {EnvelopeId}", envelope.EnvelopeId);
                        throw new NullReferenceException("Unable To Pull Envelope Record");
                    }

                    if (envelope is not null)
                    {
                        return new UpdatedEnvelope
                        {
                            Id = envDbRecord.Id,
                            EnvelopeStatus = (EnvelopeStatus)Enum.Parse(typeof(EnvelopeStatus),
#pragma warning disable CA1304
                                char.ToUpper(envelope.Status[0]) + envelope.Status[1..]),
#pragma warning restore CA1304
                            AgreementId = envDbRecord.AgreementId,
                            EnvelopeId = envDbRecord.EnvelopeId,
                            CreatedOn = envDbRecord.CreatedOn,
                            LastModifiedOnUtc =
                                DateTime.TryParse(envelope.LastModifiedDateTime, out DateTime dtLastModified)
                                    ? dtLastModified
                                    : null,
                            InitialSentOnUtc =
                                DateTime.TryParse(envelope.InitialSentDateTime, out DateTime dtInitialSent)
                                    ? dtInitialSent
                                    : null,
                            SentOnUtc = DateTime.TryParse(envelope.SentDateTime, out DateTime dtSent) ? dtSent : null,
                            LastStatusChangedOnUtc =
                                DateTime.TryParse(envelope.LastModifiedDateTime, out DateTime dtLastStatus)
                                    ? dtLastStatus
                                    : null,
                            CompletedOnUtc = DateTime.TryParse(envelope.CompletedDateTime, out DateTime dtCompleted)
                                ? dtCompleted
                                : null,
                            DeliveredOnUtc = DateTime.TryParse(envelope.DeliveredDateTime, out DateTime dtDelivered)
                                ? dtDelivered
                                : null,
                            ExpiringOnUtc = DateTime.TryParse(envelope.ExpireDateTime, out DateTime dtExpiring)
                                ? dtExpiring
                                : null,
                            VoidedOnUtc = DateTime.TryParse(envelope.VoidedDateTime, out DateTime dtVoided)
                                ? dtVoided
                                : null,
                            VoidReason = string.IsNullOrEmpty(envelope.VoidedReason)
                                ? null
                                : new VoidReason(envelope.VoidedReason),
                            AutoRespondReason = new AutoRespondReason("")
                        };
                    }

                    logger.LogWarning("Envelope returned Null, Unable To Process");
                    throw new NullReferenceException();

                });

                await UpdateEnvelopeAsync(updatedEnvelope);
            }
        }

        await unitOfWork.SaveChangesAsync();
        transaction.Commit();
        await cacheService.SetAsync("LastQueryDateTime", dateTimeProvider.UtcNow.ToString("s"),
            TimeSpan.FromHours(_docuSignOptions.ExpireInHoursCache));
        logger.LogInformation("Envelope Status Processed Completed");
    }

    private async Task<IReadOnlyList<EnvelopeResponse>> GetProcessingEnvelopeIdsAsync(
        IDbConnection connection,
        IDbTransaction transaction)
    {
        string sql = $"""
                      SELECT TOP {_docuSignOptions.BatchSize} [Id]
                      ,[EnvelopeStatus]
                      ,[AgreementId]
                      ,[EnvelopeId]
                      ,[CreatedOnUtc]
                      ,[LastModifiedOnUtc]
                      ,[InitialSentOnUtc]
                      ,[SentOnUtc]
                      ,[LastStatusChangedOnUtc]
                      ,[CompletedOnUtc]
                      ,[DeliveredOnUtc]
                      ,[ExpiringOnUtc]
                      ,[VoidedOnUtc]
                      ,[VoidReason]
                      ,[AutoRespondReason]
                        FROM [TPF_Envelopes]
                        WHERE
                      	EnvelopeId IS NOT NULL
                      	AND EnvelopeStatus IN (3,4)
                      """;

        IEnumerable<EnvelopeResponse> envelopeResponses = await connection.QueryAsync<EnvelopeResponse>(
            sql,
            transaction: transaction);

        return envelopeResponses.ToList();
    }

    private async Task UpdateEnvelopeAsync(
        IEnumerable<UpdatedEnvelope> updatedEnvelope)
    {
        foreach (UpdatedEnvelope envelope in updatedEnvelope)
        {
            Envelope? envRecord = await envelopeRepository.GetByIdAsync(envelope.Id);
            envRecord?.SetUpdatedValues(
                    envelope.EnvelopeStatus,
                    envelope.EnvelopeId,
                    envelope.LastModifiedOnUtc,
                    envelope.InitialSentOnUtc,
                    envelope.SentOnUtc,
                    envelope.LastStatusChangedOnUtc,
                    envelope.CompletedOnUtc,
                    envelope.DeliveredOnUtc,
                    envelope.ExpiringOnUtc,
                    envelope.VoidedOnUtc,
                    envelope.VoidReason,
                    envelope.AutoRespondReason);
        }
    }

    private class UpdatedEnvelope
    {
        public Guid Id { get; init; }
        public EnvelopeStatus EnvelopeStatus { get; init; }
        public Guid AgreementId { get; init; }
        public Guid? EnvelopeId { get; init; }
        public DateTime CreatedOn { get; init; }
        public DateTime? LastModifiedOnUtc { get; init; }
        public DateTime? InitialSentOnUtc { get; init; }
        public DateTime? SentOnUtc { get; init; }
        public DateTime? LastStatusChangedOnUtc { get; init; }
        public DateTime? CompletedOnUtc { get; init; }
        public DateTime? DeliveredOnUtc { get; init; }
        public DateTime? ExpiringOnUtc { get; init; }
        public DateTime? VoidedOnUtc { get; init; }
        public VoidReason? VoidReason { get; init; }
        public AutoRespondReason? AutoRespondReason { get; init; }
     
        
    }
}
