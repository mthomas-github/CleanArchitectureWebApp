using ThirdPartyFreight.Domain.Notes;

namespace ThirdPartyFreight.Api.Controllers.Notes;

public sealed record AddNoteRequest(Guid AgreementId, string NoteContent, NoteType NoteType);
