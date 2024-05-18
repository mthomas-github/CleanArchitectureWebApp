using ThirdPartyFreight.Domain.Abstractions;

namespace ThirdPartyFreight.Domain.WorkflowTask.Events;

public sealed record WorkFlowTaskUpdatedDomainEvent(Guid WorkFlowId) : IDomainEvent;
