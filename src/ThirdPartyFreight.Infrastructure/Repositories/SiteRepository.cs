using ThirdPartyFreight.Domain.Sites;

namespace ThirdPartyFreight.Infrastructure.Repositories;

internal sealed class SiteRepository : Repository<Site>, ISiteRepository
{
    public SiteRepository(ApplicationDbContext dbContext)
        : base(dbContext)
    {
    }
}