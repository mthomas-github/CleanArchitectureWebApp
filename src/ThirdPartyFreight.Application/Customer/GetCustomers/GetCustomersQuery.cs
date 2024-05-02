using ThirdPartyFreight.Application.Abstractions.Caching;
using ThirdPartyFreight.Application.Abstractions.Messaging;
using ThirdPartyFreight.Application.Shared;

namespace ThirdPartyFreight.Application.Customer.GetCustomers;

public sealed record GetCustomersQuery() : ICachedQuery<IReadOnlyList<CustomerResponse>>
{
    public string CacheKey => "GetCustomersQuery";
    public TimeSpan? Expiration => TimeSpan.FromMinutes(60);
}

