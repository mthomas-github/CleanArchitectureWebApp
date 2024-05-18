using System;
using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.Approvals;

namespace ThirdPartyFreight.Domain.WorkflowTask;

public sealed class WorkFlowTask : Entity
{

    private WorkFlowTask(
        Guid id,
        string externalId,
        string processId,
        string name,
        ApproverType approver,
        Guid agreementId,
        bool isCompleted,
        DateTimeOffset createdAt,
        DateTimeOffset? completedAt)
        : base(id)
    {
        Id = id;
        ExternalId = externalId;
        ProcessId = processId;
        Name = name;
        Approver = approver;
        IsCompleted = isCompleted;
        CreatedAt = createdAt;
        CompletedAt = completedAt;
        AgreementId = agreementId;
    }

    private WorkFlowTask() { }

    /// <summary>
    /// An external ID that can be used to reference the task.
    /// </summary>
    public string ExternalId { get; private set; } = default!;

    /// <summary>
    /// The ID of the onboarding process that the task belongs to.
    /// </summary>
    public string ProcessId { get; private set; } = default!;

    /// <summary>
    /// The name of the task.
    /// </summary>
    public string Name { get; private set; } = default!;

    /// <summary>
    /// The task description.
    /// </summary>
    public ApproverType Approver { get; private set; } = default!;

    /// <summary>
    /// The Agreement that the approval task belongs to.
    /// </summary>
    public Guid AgreementId { get; private set; }

    /// <summary>
    /// Whether the task has been completed.
    /// </summary>
    public bool IsCompleted { get; private set; }

    /// <summary>
    /// The date and time when the task was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; private set; }

    /// <summary>
    /// The date and time when the task was completed.
    /// </summary>
    public DateTimeOffset? CompletedAt { get; private set; }


    public static WorkFlowTask Create(
        string externalId,
        string processId,
        string name,
        ApproverType approver,
        Guid agreementId,
        DateTimeOffset createdAt)
    {
        var workflowTask = new WorkFlowTask(Guid.NewGuid(), externalId, processId, name, approver, agreementId, false, createdAt,
            null);

        return workflowTask;
    }


    public static WorkFlowTask Complete(
        WorkFlowTask workFlowTask)
    {
        var updated = new WorkFlowTask(
            workFlowTask.Id, 
            workFlowTask.ExternalId, 
            workFlowTask.ProcessId, 
            workFlowTask.Name, 
            workFlowTask.Approver, 
            workFlowTask.AgreementId, 
            true, 
            workFlowTask.CreatedAt, 
            DateTimeOffset.Now );

        return updated;
    }

}
