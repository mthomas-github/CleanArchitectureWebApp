using ThirdPartyFreight.Domain.Abstractions;

namespace ThirdPartyFreight.Domain.Envelopes.Events;

public sealed record EnvelopeUpdatedDomainEvent(Guid EnvelopeId) : IDomainEvent;
