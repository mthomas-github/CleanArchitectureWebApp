using ThirdPartyFreight.Domain.Documents;

namespace ThirdPartyFreight.Api.Controllers.Documents;

public sealed record AddDocumentRequest(Guid AgreementId, string DocumentName, string DocumentData, DocumentType DocumentType);
