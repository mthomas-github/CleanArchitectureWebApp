using ThirdPartyFreight.Domain.Documents;

namespace ThirdPartyFreight.Web.Features.Agreements.Models;

public sealed class DocumentResponse
{
    public Guid DocumentId { get; set; }
    public string DocumentName { get; set; }
    public string DocumentData { get; set; }
    public DocumentType DocumentType { get; set; }
}
