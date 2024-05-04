using Microsoft.EntityFrameworkCore;
using ThirdPartyFreight.Domain.Sites;

namespace ThirdPartyFreight.Infrastructure.Repositories;

internal sealed class SiteRepository : Repository<Site>, ISiteRepository
{
    public SiteRepository(ApplicationDbContext dbContext)
        : base(dbContext)
    {
    }

    public void Add(IEnumerable<Site> sites)
    {
        DbContext.AddRangeAsync(sites);
    }

    public async Task<IEnumerable<Site>> GetSitesAsyncByAgreementId(
        Guid agreementId,
        CancellationToken cancellationToken = default)
    {
        return await DbContext
            .Set<Site>()
            .Where(site => site.AgreementId == agreementId)
            .ToListAsync(cancellationToken);
    }
}
