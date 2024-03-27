using ThirdPartyFreight.Domain.Abstractions;

namespace ThirdPartyFreight.Domain.Notes.Events;

public sealed record NoteCreatedDomainEvent(Guid NoteId) : IDomainEvent;