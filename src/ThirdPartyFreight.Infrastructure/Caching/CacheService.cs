using System.Buffers;
using System.Text.Json;
using ThirdPartyFreight.Application.Abstractions.Caching;
using Microsoft.Extensions.Caching.Distributed;

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
       byte[]? bytes = await _cache.GetAsync(cacheKey, cancellationToken);

       return bytes is null ? default : Deserialize<T>(bytes);
    }

    private static T Deserialize<T>(byte[] bytes)
    {
        return JsonSerializer.Deserialize<T>(bytes)!;
    }

    public Task SetAsync<T>(
        string cacheKey, 
        T value, 
        TimeSpan? expiration = null, 
        CancellationToken cancellationToken = default)
    {
        byte[] bytes = Serialize(value);

        return _cache.SetAsync(cacheKey, bytes,CacheOptions.Create(expiration), cancellationToken);
    }

    private static byte[] Serialize<T>(T value)
    {
        var buffer = new ArrayBufferWriter<byte>();

        using var writer = new Utf8JsonWriter(buffer);

        JsonSerializer.Serialize(writer, value);

        return buffer.WrittenSpan.ToArray();
    }

    public Task RemoveAsync(string cacheKey, CancellationToken cancellationToken = default)
    {
        return _cache.RemoveAsync(cacheKey, cancellationToken);
    }
}
