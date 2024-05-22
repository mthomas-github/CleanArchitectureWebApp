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

        const string sql = $"""
                            SELECT
                                ApprovalId,
                                TaskId,
                                AgreementId,
                                AgreementType,
                                WorkFlowTaskId,
                                Approver,
                                FirstApprovalStart,
                                FirstApprovalEnd,
                                SecondApprovalStart,
                                SecondApprovalEnd,
                                ThirdApprovalStart,
                                ThirdApprovalEnd,
                                CompletedOn
                            FROM
                               View_TPFApprovals
                            WHERE 
                                CompletedOn is null;
                            """;

        IEnumerable<ApprovalResponse> result = await connection.QueryAsync<ApprovalResponse>(sql);
        return result.ToList();

    }
}
