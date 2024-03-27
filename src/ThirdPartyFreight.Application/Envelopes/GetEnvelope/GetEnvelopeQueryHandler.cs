using System.Data;
using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.Envelopes;
using Dapper;
using ThirdPartyFreight.Application.Abstractions.Data;
using ThirdPartyFreight.Application.Abstractions.Messaging;
using ThirdPartyFreight.Application.Shared;

namespace ThirdPartyFreight.Application.Envelopes.GetEnvelope;

internal sealed class GetEnvelopeQueryHandler : IQueryHandler<GetEnvelopeQuery, EnvelopeResponse>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetEnvelopeQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<EnvelopeResponse>> Handle(GetEnvelopeQuery request, CancellationToken cancellationToken)
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

        const string sql = """
                           SELECT
                            e.Id EnvelopeId,
                            e.EnvelopeId DocuSignId,
                            e.EnvelopeStatus,
                            e.AutoRespondReason,
                            e.VoidReason,
                            e.SentOnUtc,
                            e.CompletedOnUtc,
                            e.CreatedOnUtc CreatedOn,
                            e.DeliveredOnUtc,
                            e.ExpiringOnUtc,
                            e.InitialSentOnUtc,
                            e.LastModifiedOnUtc,
                            e.LastStatusChangedOnUtc
                           FROM
                            TPF_Envelopes e
                           WHERE
                            e.Id = @EnvelopeId;
                           """;

        EnvelopeResponse? result = await connection.QueryFirstOrDefaultAsync<EnvelopeResponse>(sql, new { request.EnvelopeId });

        return result ?? Result.Failure<EnvelopeResponse>(EnvelopeErrors.NotFound);
    }
}
