using ThirdPartyFreight.Domain.Abstractions;

namespace ThirdPartyFreight.Domain.WorkflowTask.Events;

public sealed record WorkFlowTaskCancelledDomainEvent(Guid WorkFlowTaskId) : IDomainEvent;
