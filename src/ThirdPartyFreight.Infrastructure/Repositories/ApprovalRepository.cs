using ThirdPartyFreight.Domain.Approvals;

namespace ThirdPartyFreight.Infrastructure.Repositories;

internal sealed class ApprovalRepository : Repository<Approval>, IApprovalRepository
{
    public ApprovalRepository(ApplicationDbContext dbContext) 
        : base(dbContext)
    {
    }

    public void Update(Approval approval)
    {
        DbContext.Set<Approval>().Update(approval);
    }
}
