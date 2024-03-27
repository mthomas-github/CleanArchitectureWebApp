using ThirdPartyFreight.Domain.Notes;
using ThirdPartyFreight.Application.Abstractions.Messaging;

namespace ThirdPartyFreight.Application.Notes.AddNote;

public sealed record AddNoteCommand(
    Guid AgreementId,
    string NoteContent,
    NoteType NoteType) : ICommand;
