using ThirdPartyFreight.Domain.Documents;

namespace ThirdPartyFreight.Web.Features.Agreements.Models;

public sealed record AddDocumentRequest(string AgreementId, string DocumentName, string DocumentData, DocumentType DocumentType);
