using ThirdPartyFreight.Domain.Abstractions;

namespace ThirdPartyFreight.Domain.Envelopes.Events;

public sealed record EnvelopeCreatedDomainEvent(Guid EnvelopeId) : IDomainEvent;