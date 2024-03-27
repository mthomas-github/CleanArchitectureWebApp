using System.Data;
using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.Approvals;
using Dapper;
using ThirdPartyFreight.Application.Abstractions.Data;
using ThirdPartyFreight.Application.Abstractions.Messaging;

namespace ThirdPartyFreight.Application.Approvals.GetApproval;

internal sealed class GetApprovalQueryHandler : IQueryHandler<GetApprovalQuery, ApprovalResponse>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetApprovalQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<ApprovalResponse>> Handle(GetApprovalQuery request, CancellationToken cancellationToken)
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

        const string sql = """
                           SELECT
                                Id,
                                AgreementId,
                                CreatedOnUtc,
                                FirstApprovalOnUtc,
                                FirstApprovalEndUtc,
                                SecondApprovalOnUtc,
                                SecondApprovalEndUtc,
                                ThirdApprovalOnUtc,
                                ThirdApprovalEndUtc,
                                CompletedOn
                           FROM
                            TPF_Approvals
                           WHERE
                            Id = @ApprovalId;
                           """;

        ApprovalResponse? result = await connection.QueryFirstOrDefaultAsync<ApprovalResponse>(sql, new { request.ApprovalId });

        return result ?? Result.Failure<ApprovalResponse>(ApprovalErrors.NotFound);
    }
}
