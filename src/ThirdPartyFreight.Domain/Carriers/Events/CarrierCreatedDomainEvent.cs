using ThirdPartyFreight.Domain.Abstractions;

namespace ThirdPartyFreight.Domain.Carriers.Events;

public sealed record CarrierCreatedDomainEvent(Guid CarrierId) : IDomainEvent;