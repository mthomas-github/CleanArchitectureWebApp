using ThirdPartyFreight.Domain.Notes;

namespace ThirdPartyFreight.Web.Features.Approvals.Models;

public sealed record AddNoteRequest(Guid AgreementId, string NoteContent, NoteType NoteType);
