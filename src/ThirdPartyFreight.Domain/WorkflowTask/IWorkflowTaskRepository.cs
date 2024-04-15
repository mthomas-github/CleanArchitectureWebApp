﻿namespace ThirdPartyFreight.Domain.WorkflowTask;

public interface IWorkFlowTaskRepository
{
    Task<WorkFlowTask?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    void Add(WorkFlowTask workFlowTask);
    void Update(WorkFlowTask workFlowTask);
}
