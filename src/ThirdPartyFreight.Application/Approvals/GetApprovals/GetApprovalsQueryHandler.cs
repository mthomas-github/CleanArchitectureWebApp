using System.Collections;
using System.Data;
using Dapper;
using ThirdPartyFreight.Application.Abstractions.Data;
using ThirdPartyFreight.Application.Abstractions.Messaging;
using ThirdPartyFreight.Application.Shared;
using ThirdPartyFreight.Domain.Abstractions;

namespace ThirdPartyFreight.Application.Approvals.GetApprovals;

internal sealed class GetApprovalsQueryHandler : IQueryHandler<GetApprovalsQuery, IReadOnlyList<ApprovalResponse>>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetApprovalsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }
    public async Task<Result<IReadOnlyList<ApprovalResponse>>> Handle(GetApprovalsQuery request, CancellationToken cancellationToken)
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
                            TPF_Approvals;
                           """;

        IEnumerable<ApprovalResponse> result = await connection.QueryAsync<ApprovalResponse>(sql);
        return result.ToList();

    }
}
