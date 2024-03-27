using ThirdPartyFreight.Application.Abstractions.Messaging;
using ThirdPartyFreight.Application.Shared;

namespace ThirdPartyFreight.Application.Notes.GetNote;

public sealed record GetNoteQuery(Guid NoteId) : IQuery<NoteResponse>;
