using ThirdPartyFreight.Domain.Notes;

namespace ThirdPartyFreight.Web.Features.Approvals.Models;

public record AddNoteRequest(Guid AgreementId, string NoteContent, NoteType NoteType);
