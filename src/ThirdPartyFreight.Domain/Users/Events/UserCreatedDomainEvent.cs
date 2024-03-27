using ThirdPartyFreight.Domain.Abstractions;

namespace ThirdPartyFreight.Domain.Users.Events;

public sealed record UserCreatedDomainEvent(Guid UserId) : IDomainEvent;