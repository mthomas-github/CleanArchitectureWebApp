using ThirdPartyFreight.Application.Abstractions.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;


namespace ThirdPartyFreight.Infrastructure.Caching;

internal sealed class CacheService : ICacheService
{
    private readonly IDistributedCache _cache;

    public CacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<T?> GetAsync<T>(string cacheKey, CancellationToken cancellationToken = default)
    {
        string? jsonString = await _cache.GetStringAsync(cacheKey, cancellationToken);

        return jsonString is null ? default : Deserialize<T>(jsonString);
    }

    private static T Deserialize<T>(string jsonString)
    {
        return JsonConvert.DeserializeObject<T>(jsonString);
    }

    public Task SetAsync<T>(
        string cacheKey, 
        T value, 
        TimeSpan? expiration = null, 
        CancellationToken cancellationToken = default)
    {
        string jsonString = Serialize(value);

        return _cache.SetStringAsync(cacheKey, jsonString, CacheOptions.Create(expiration), cancellationToken);
    }

    private static string Serialize<T>(T value)
    {
        return JsonConvert.SerializeObject(value);
    }

    public Task RemoveAsync(string cacheKey, CancellationToken cancellationToken = default)
    {
        return _cache.RemoveAsync(cacheKey, cancellationToken);
    }
}
