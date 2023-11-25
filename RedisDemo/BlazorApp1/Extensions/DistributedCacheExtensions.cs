using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace BlazorApp1.Extensions;

public static class DistributedCacheExtensions
{
    public static async Task SetRecordAsync<T>(this IDistributedCache cache,
        string key,
        T record,
        TimeSpan? absoluteExpireTime = null,
        TimeSpan? unusedExpireTime = null)
    {
        var options = new DistributedCacheEntryOptions();

        options.AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(60);
        options.SlidingExpiration = unusedExpireTime;

        var jsonRecord = JsonSerializer.Serialize(record);
        await cache.SetStringAsync(key, jsonRecord, options);
    }

    public static async Task<T> GetRecordAsync<T>(this IDistributedCache cache, string key)
    {
        var jsonRecord = await cache.GetStringAsync(key);

        if (jsonRecord is null)
        {
            return default(T);
        }

        return JsonSerializer.Deserialize<T>(jsonRecord);

    }
}
