namespace ThirdPartyFreight.Domain.Documents;

public interface IDocumentRepository
{
    Task<Document?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    void Add(Document document);
}