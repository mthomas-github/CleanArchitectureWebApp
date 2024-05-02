namespace ThirdPartyFreight.Domain.Customer;
public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    void Add(Customer agreement);
}
