using ThirdPartyFreight.Domain.Abstractions;

namespace ThirdPartyFreight.Domain.Agreements.Events;

public record AgreementUpdatedDomainEvent(Guid AgreementId) : IDomainEvent;
