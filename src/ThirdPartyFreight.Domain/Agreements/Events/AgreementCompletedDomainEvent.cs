using ThirdPartyFreight.Domain.Abstractions;

namespace ThirdPartyFreight.Domain.Agreements.Events;

public sealed record AgreementCompletedDomainEvent(Guid AgreementId) : IDomainEvent;