using Microsoft.EntityFrameworkCore;
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

    public async Task<Approval?> GetByTaskIdAsync(string taskId, CancellationToken cancellationToken = default)
    {
        return await DbContext
            .Set<Approval>()
            .FirstOrDefaultAsync(approval => approval.TaskId == taskId, cancellationToken);
    }

    public async Task<Approval?> GetByAgreementIdAsync(Guid agreementId, CancellationToken cancellationToken = default)
    {
        return await DbContext
            .Set<Approval>()
            .FirstOrDefaultAsync(approval => approval.AgreementId == agreementId, cancellationToken);
    }
}
