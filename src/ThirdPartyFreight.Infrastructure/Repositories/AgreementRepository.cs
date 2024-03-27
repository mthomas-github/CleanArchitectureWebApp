using ThirdPartyFreight.Domain.Agreements;
using Microsoft.EntityFrameworkCore;

namespace ThirdPartyFreight.Infrastructure.Repositories;

internal sealed class AgreementRepository : Repository<Agreement>, IAgreementRepository
{
    public AgreementRepository(ApplicationDbContext dbContext)
        : base(dbContext)
    {
    }
}
