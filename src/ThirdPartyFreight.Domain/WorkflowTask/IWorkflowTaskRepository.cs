namespace ThirdPartyFreight.Domain.WorkflowTask;

public interface IWorkflowTaskRepository
{
    Task<WorkflowTask?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    void Add(WorkflowTask workflowTask);
}
