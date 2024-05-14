using Microsoft.EntityFrameworkCore;
using ThirdPartyFreight.Domain.WorkflowTask;

namespace ThirdPartyFreight.Infrastructure.Repositories;

internal sealed class WorkFlowTaskRepository : Repository<WorkFlowTask>, IWorkFlowTaskRepository
{
    public WorkFlowTaskRepository(ApplicationDbContext dbContext)
        : base(dbContext)
    { }

    public void Update(WorkFlowTask workFlowTask)
    {
        DbContext.Set<WorkFlowTask>().Update(workFlowTask);
    }
    
    public async Task<IEnumerable<WorkFlowTask>> GetWorkFlowTaskAsyncByAgreementId(
        Guid agreementId,
        CancellationToken cancellationToken = default)
    {
        return await DbContext
            .Set<WorkFlowTask>()
            .Where(wf => wf.AgreementId == agreementId)
            .ToListAsync(cancellationToken);
    }
}
