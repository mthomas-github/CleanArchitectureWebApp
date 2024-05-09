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
using ThirdPartyFreight.Domain.Envelopes;
using Envelope = DocuSign.eSign.Model.Envelope;

namespace ThirdPartyFreight.Infrastructure.DocuSign.BackgroundJobs;

[DisallowConcurrentExecution]
internal sealed class ProcessStatusUpdateJob(
    ISqlConnectionFactory sqlConnectionFactory,
    IOptions<DocuSignOptions> docuSignOptions,
    ILogger<ProcessStatusUpdateJob> logger,
    IDocuSignService docuSignService,
    ICacheService cacheService,
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
                StatusToInclude: "completed,declined,delivered,sent,signed,timedout,void",
                FromDate: lastQueryDateTime,
                ToDate: dateTimeProvider.UtcNow.ToString("s"));

            EnvelopesInformation response = await docuSignService.GetEnvelopesInformation(request);
            string resultSize = response.ResultSetSize;
            if (int.Parse(resultSize, CultureInfo.InvariantCulture) > 0)
            {
                IEnumerable<UpdatedEnvelope> updatedEnvelope = response.Envelopes.Select(envelope => new UpdatedEnvelope
                {
                    EnvelopeStatus = (EnvelopeStatus)Enum.Parse(typeof(EnvelopeStatus), envelope.Status),
                    VoidedOnUtc = DateTime.Parse(envelope.VoidedDateTime, CultureInfo.InvariantCulture),
                    VoidReason = envelope.VoidedReason,
                    ExpiringOnUtc = DateTime.Parse(envelope.ExpireDateTime, CultureInfo.InvariantCulture),
                    Id = processingEnvelopeIds.FirstOrDefault(x => x.EnvelopeId.ToString() == envelope.EnvelopeId)!
                        .Id
                });

                await UpdateEnvelopeAsync(connection, transaction, updatedEnvelope);
            }
            // Update Envelope Status
        }

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
                      SELECT TOP ${_docuSignOptions.BatchSize} [Id]
                            ,[EnvelopeStatus]
                            ,[AgreementId]
                            ,[EnvelopeId]
                            ,[LastStatusChangedOnUtc]
                        FROM [csddevapps].[dbo].[TPF_Envelopes]
                        WHERE
                      	EnvelopeId IS NOT NULL
                      	AND EnvelopeStatus IN (3,4)
                      """;

        IEnumerable<EnvelopeResponse> envelopeResponses = await connection.QueryAsync<EnvelopeResponse>(
            sql,
            transaction: transaction);

        return envelopeResponses.ToList();
    }

    private static async Task UpdateEnvelopeAsync(
        IDbConnection connection,
        IDbTransaction transaction,
        IEnumerable<UpdatedEnvelope> updatedEnvelope)
    {
        
        const string sql = """
                           UPDATE[csddevapps].[dbo].[TPF_Envelopes]
                           SET EnvelopeStatus = @EnvelopeStatus,
                               VoidedOnUTC =
                                  CASE WHEN EnvelopeStatus IN (5, 6) THEN @VoidedOnUTC ELSE NULL END,
                               VoidReason =
                                  CASE WHEN EnvelopeStatus IN (5, 6) THEN @VoidReason ELSE NULL END,
                               ExpiringOnUtc =
                                  CASE
                                     WHEN ExpiringOnUtc IS NOT NULL AND ExpiringOnUtc != ''
                                     THEN
                                        @ExpiringOnUtc
                                     ELSE
                                        NULL
                                  END
                           WHERE Id = @Id
                           """;

        await connection.ExecuteAsync(sql, updatedEnvelope, transaction: transaction);
    }

    private class UpdatedEnvelope
    {
        public EnvelopeStatus EnvelopeStatus { get; set; }
        public DateTime? VoidedOnUtc { get; set; }
        public string? VoidReason { get; set; }
        public DateTime? ExpiringOnUtc { get; set; }
        public Guid Id { get; set; }
     
        
    }
}
