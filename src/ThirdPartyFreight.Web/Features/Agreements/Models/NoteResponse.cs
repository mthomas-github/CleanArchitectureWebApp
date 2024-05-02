using ThirdPartyFreight.Domain.Notes;

namespace ThirdPartyFreight.Web.Features.Agreements.Models;

public sealed class NoteResponse
{
    public Guid NoteId { get; set; }
    public string NoteContent { get; set; }
    public DateTime CreatedAt { get; set; }
    public NoteType NoteType { get; set; }
}
