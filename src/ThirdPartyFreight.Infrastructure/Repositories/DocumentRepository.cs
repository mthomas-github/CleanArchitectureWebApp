using ThirdPartyFreight.Domain.Documents;

namespace ThirdPartyFreight.Infrastructure.Repositories;

internal sealed class DocumentRepository : Repository<Document>, IDocumentRepository
{
    public DocumentRepository(ApplicationDbContext dbContext) 
        : base(dbContext)
    {
    }
}