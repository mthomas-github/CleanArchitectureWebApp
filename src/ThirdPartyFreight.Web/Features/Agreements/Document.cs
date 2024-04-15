using ThirdPartyFreight.Domain.Documents;

namespace ThirdPartyFreight.Web.Features.Agreements;

public class Document
{
    public Guid AgreementId { get; set; }
    public string DocumentName { get; set; }
    public string DocumentData { get; set; }
    public DocumentType DocumentType { get; set; }
}
