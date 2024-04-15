using System.Data;
using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.Approvals;
using Dapper;
using ThirdPartyFreight.Application.Abstractions.Data;
using ThirdPartyFreight.Application.Abstractions.Messaging;

namespace ThirdPartyFreight.Application.Approvals.GetApproval;

internal sealed class GetApprovalQueryHandler : IQueryHandler<GetApprovalQuery, Approval>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetApprovalQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<Approval>> Handle(GetApprovalQuery request, CancellationToken cancellationToken)
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

        Approval? result = await connection.QueryFirstOrDefaultAsync<Approval>(sql, new { request.ApprovalId });

        return result ?? Result.Failure<Approval>(ApprovalErrors.NotFound);
    }
}
