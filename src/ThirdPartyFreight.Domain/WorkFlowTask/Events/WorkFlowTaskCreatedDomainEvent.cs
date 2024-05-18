using ThirdPartyFreight.Domain.Abstractions;

namespace ThirdPartyFreight.Domain.WorkflowTask.Events;

public sealed record WorkFlowTaskCreatedDomainEvent(Guid WorkFlowId) : IDomainEvent;
