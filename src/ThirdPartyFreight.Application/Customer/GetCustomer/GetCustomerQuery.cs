using ThirdPartyFreight.Application.Abstractions.Caching;
using ThirdPartyFreight.Application.Shared;

namespace ThirdPartyFreight.Application.Customer.GetCustomer;

public sealed record GetCustomerQuery(string CustomerNumber) : ICachedQuery<CustomerSiteResponse>
{
    public string CacheKey => $"GetCustomerQuery-{CustomerNumber}";
    public TimeSpan? Expiration => TimeSpan.FromMinutes(60);
}
