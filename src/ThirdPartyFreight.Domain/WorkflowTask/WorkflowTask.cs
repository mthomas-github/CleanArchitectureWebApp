namespace ThirdPartyFreight.Domain.WorkflowTask;

public sealed class WorkflowTask
{

    private WorkflowTask(
        long id,
        string externalId,
        string processId,
        string name,
        string description,
        bool isCompleted,
        DateTimeOffset createdAt,
        DateTimeOffset? completedAt)
    {
        Id = id;
        ExternalId = externalId;
        ProcessId = processId;
        Name = name;
        Description = description;
        IsCompleted = isCompleted;
        CreatedAt = createdAt;
        CompletedAt = completedAt;
    }

    private WorkflowTask() { }

    /// <summary>
    /// The ID of the task.
    /// </summary>
    public long Id { get; private set; }

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
    public string Description { get; private set; } = default!;

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


    public static WorkflowTask Create(
        string externalId,
        string processId,
        string name,
        string description,
        DateTimeOffset createdAt)
    {
        var workflowTask = new WorkflowTask(
                       0,
                                  externalId,
                                  processId,
                                  name,
                                  description,
                                  false,
                                  createdAt,
                                  null
                              );

        return workflowTask;
    }
}
