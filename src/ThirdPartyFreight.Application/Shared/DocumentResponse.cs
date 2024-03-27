using ThirdPartyFreight.Domain.Documents;

namespace ThirdPartyFreight.Application.Shared;

public sealed class DocumentResponse
{
    public Guid DocumentId { get; init; }
    public string DocumentName { get; init; }
    public string DocumentData { get; init; }
    public DocumentType DocumentType { get; init; }
}
