using System.Data;
using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.Audits;
using Dapper;
using ThirdPartyFreight.Application.Abstractions.Data;
using ThirdPartyFreight.Application.Abstractions.Messaging;

namespace ThirdPartyFreight.Application.Audits.GetAudit;

internal sealed class GetAuditQueryHandler : IQueryHandler<GetAuditQuery, AuditResponse>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetAuditQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<AuditResponse>> Handle(GetAuditQuery request, CancellationToken cancellationToken)
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

        const string sql = """
                           SELECT
                            Id,
                            AgreementId,
                            AuditInfo_AuditDateUtc AuditDateUtc,
                            AuditInfo_IsAuditActive IsAuditActive,
                            AuditInfo_AuditCompleteDateUtc AuditCompleteDateUtc
                           FROM
                            TPF_Audits
                           WHERE
                            Id = @AuditId;
                           """;

        AuditResponse? result = await connection.QueryFirstOrDefaultAsync<AuditResponse>(sql, new { request.AuditId });

        return result ?? Result.Failure<AuditResponse>(AuditErrors.NotFound);
    }
}
