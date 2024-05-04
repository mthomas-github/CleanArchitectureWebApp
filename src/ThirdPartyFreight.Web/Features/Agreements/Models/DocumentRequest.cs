using ThirdPartyFreight.Domain.Documents;

namespace ThirdPartyFreight.Web.Features.Agreements.Models;

public class DocumentRequest
{
    public string AgreementId { get; set; }
    public string DocumentName { get; set; }
    public string DocumentData { get; set; }
    public DocumentType DocumentType { get; set; }

}
