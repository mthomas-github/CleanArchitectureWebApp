using ThirdPartyFreight.Domain.Abstractions;

namespace ThirdPartyFreight.Domain.Documents.Events;

public sealed record DocumentCreatedDomainEvent(Guid DocumentId) : IDomainEvent;