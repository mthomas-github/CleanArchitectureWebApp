using ThirdPartyFreight.Domain.Audits;

namespace ThirdPartyFreight.Infrastructure.Repositories;

internal sealed class AuditRepository : Repository<Audit>, IAuditRepository
{
    public AuditRepository(ApplicationDbContext dbContext)
        : base(dbContext)
    {
    }

    public void Update(Audit audit)
    {
        DbContext.Set<Audit>().Update(audit);
    }
}
