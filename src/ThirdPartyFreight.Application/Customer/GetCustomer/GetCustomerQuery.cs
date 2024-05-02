using ThirdPartyFreight.Application.Abstractions.Messaging;
using ThirdPartyFreight.Application.Shared;

namespace ThirdPartyFreight.Application.Customer.GetCustomer;

public sealed record GetCustomerQuery(int CustomerNumber) : IQuery<CustomerResponse>;
