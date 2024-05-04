
namespace ThirdPartyFreight.Domain.Sites;

public interface ISiteRepository
{
    Task<Site?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    void Add(Site site);
    void Add(IEnumerable<Site> sites);
}
