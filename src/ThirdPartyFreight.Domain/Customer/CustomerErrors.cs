using ThirdPartyFreight.Domain.Abstractions;

namespace ThirdPartyFreight.Domain.Customer;

public static class CustomerErrors
{
    public static readonly Error NotFound = new(
               "Customer.NotFound",
                      "The customer with the specified identifier is not found.");
}
