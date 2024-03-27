using ThirdPartyFreight.Domain.Carriers;

namespace ThirdPartyFreight.Infrastructure.Repositories;

internal sealed class CarrierRepository : Repository<Carrier>, ICarrierRepository
{
    public CarrierRepository(ApplicationDbContext dbContext) 
        : base(dbContext)
    {
    }
}