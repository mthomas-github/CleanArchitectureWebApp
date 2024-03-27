using ThirdPartyFreight.Domain.Abstractions;

namespace ThirdPartyFreight.Domain.Audits.Events;

public sealed record AuditCreatedDomainEvent(Guid AuditId) : IDomainEvent;
