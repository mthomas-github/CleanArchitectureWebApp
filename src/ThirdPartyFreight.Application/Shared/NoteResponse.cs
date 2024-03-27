using ThirdPartyFreight.Domain.Notes;

namespace ThirdPartyFreight.Application.Shared;

public sealed class NoteResponse
{
    public Guid NoteId { get; init; }
    public string NoteContent { get; init; }
    public DateTime CreatedAt { get; init; }
    public NoteType NoteType { get; init; }
}
