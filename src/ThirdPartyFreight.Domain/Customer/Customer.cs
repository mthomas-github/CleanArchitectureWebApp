using ThirdPartyFreight.Domain.Abstractions;

namespace ThirdPartyFreight.Domain.Customer;

public sealed class Customer : Entity
{
    private Customer(
        Guid id,
        CustomerInfo customerInfo)
        : base(id)
    {
        CustomerInfo = customerInfo;
    }

    private Customer() { } // EF Core
    public CustomerInfo CustomerInfo { get; private set; }

    public static Customer Create(
        CustomerInfo customerInfo)
    {
        var customer = new Customer(Guid.NewGuid(), customerInfo);

        return customer;
    }

}
