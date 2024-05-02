﻿using ThirdPartyFreight.Application.Abstractions.Caching;
using ThirdPartyFreight.Application.Abstractions.Messaging;
using ThirdPartyFreight.Application.Shared;

namespace ThirdPartyFreight.Application.Customer.GetCustomer;

public sealed record GetCustomerQuery(int CustomerNumber) : ICachedQuery<CustomerResponse>
{
    public string CacheKey => $"GetCustomerQuery-{CustomerNumber}";
    public TimeSpan? Expiration => TimeSpan.FromMinutes(60);
}