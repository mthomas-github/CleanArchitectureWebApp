using ThirdPartyFreight.Domain.Abstractions;

namespace ThirdPartyFreight.Domain.Agreements.Events;

public sealed record AgreementCreatedDomainEvent(Guid AgreementId) : IDomainEvent;