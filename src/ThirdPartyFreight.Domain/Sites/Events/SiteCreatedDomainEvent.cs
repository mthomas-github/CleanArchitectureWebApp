using ThirdPartyFreight.Domain.Abstractions;

namespace ThirdPartyFreight.Domain.Sites.Events;

public sealed record SiteCreatedDomainEvent(Guid SiteId) : IDomainEvent;