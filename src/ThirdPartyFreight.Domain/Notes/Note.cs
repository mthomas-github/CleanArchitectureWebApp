using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.Notes.Events;

namespace ThirdPartyFreight.Domain.Notes;

public sealed class Note : Entity
{
    private Note(
        Guid id,
        Guid agreementId,
        Content content,
        DateTime createdOnUtc,
        NoteType noteType) 
        : base(id)

    {
        AgreementId = agreementId;
        Content = content;
        CreatedOnUtc = createdOnUtc;
        NoteType = noteType;
    }

    private Note() { }
    
    public Guid AgreementId { get; private set; }
    public Content Content { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }
    public NoteType NoteType { get; private set; }

    public static Note Create(
        Guid agreementId,
        Content content,
        DateTime createdOnUtc,
        NoteType noteType)
    {
        var note = new Note(Guid.NewGuid(), agreementId, content, createdOnUtc, noteType);

        note.RaiseDomainEvent(new NoteCreatedDomainEvent(note.Id));
        return note;
    }
}