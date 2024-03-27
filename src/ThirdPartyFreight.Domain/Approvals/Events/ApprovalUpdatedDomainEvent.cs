using ThirdPartyFreight.Domain.Abstractions;

namespace ThirdPartyFreight.Domain.Approvals.Events;

public sealed record ApprovalUpdatedDomainEvent(Guid ApprovalId) : IDomainEvent;
