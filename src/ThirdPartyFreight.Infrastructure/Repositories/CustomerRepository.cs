using ThirdPartyFreight.Domain.Customer;

namespace ThirdPartyFreight.Infrastructure.Repositories;

internal sealed class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    public CustomerRepository(ApplicationDbContext dbContext)
        : base(dbContext)
    {
    }
}
