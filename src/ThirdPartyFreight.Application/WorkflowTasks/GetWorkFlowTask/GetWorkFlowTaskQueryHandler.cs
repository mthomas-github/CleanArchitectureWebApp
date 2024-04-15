using System.Data;
using Dapper;
using ThirdPartyFreight.Application.Abstractions.Data;
using ThirdPartyFreight.Application.Abstractions.Messaging;
using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.WorkflowTask;

namespace ThirdPartyFreight.Application.WorkflowTasks.GetWorkFlowTask;

internal sealed class GetWorkflowTaskQueryHandler : IQueryHandler<GetWorkflowTask, WorkFlowTask>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetWorkflowTaskQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<WorkFlowTask>> Handle(GetWorkflowTask request, CancellationToken cancellationToken)
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
                             TPF_WorkflowTasks
                            WHERE
                             Id = @WorkflowTaskId;
                            """;

        WorkFlowTask? result = await connection.QueryFirstOrDefaultAsync<WorkFlowTask>(sql, new { request.WorkflowTaskId });

        return result ?? Result.Failure<WorkFlowTask>(WorkFlowTaskErrors.NotFound);
    }
}
