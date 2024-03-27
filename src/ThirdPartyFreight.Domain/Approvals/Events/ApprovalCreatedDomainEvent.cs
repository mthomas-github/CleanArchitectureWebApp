using ThirdPartyFreight.Domain.Abstractions;

namespace ThirdPartyFreight.Domain.Approvals.Events;

public sealed record ApprovalCreatedDomainEvent(Guid ApprovalId) : IDomainEvent;
