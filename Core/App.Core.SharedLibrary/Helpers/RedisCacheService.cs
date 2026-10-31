using StackExchange.Redis;
using System.Text.Json;

namespace App.Core.SharedLibrary.Helpers;

public sealed class RedisCacheService
{

    private readonly IDatabase _db;

    public RedisCacheService(IConnectionMultiplexer redis)
    {
        _db = redis.GetDatabase();
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan expiry)
    {
        string json = JsonSerializer.Serialize(value);
        await _db.StringSetAsync(key, json, expiry);
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        RedisValue value = await _db.StringGetAsync(key);
        if (value.IsNullOrEmpty) return default;
        return JsonSerializer.Deserialize<T>(value!);
    }

    public async Task RemoveAsync(string key)
    {
        await _db.KeyDeleteAsync(key);
    }

public async Task<bool> AddToSetAsync(
    string key,
    string value,
    TimeSpan? expiry = null)
{
    bool added = await _db.SetAddAsync(key, value);

    if (expiry.HasValue)
    {
        await _db.KeyExpireAsync(key, expiry);
    }

    return added;
}

    public Task<bool> RemoveFromSetAsync(string key, string value)
    {
        return _db.SetRemoveAsync(key, value);
    }

    public Task<bool> IsInSetAsync(string key, string value)
    {
        return _db.SetContainsAsync(key, value);
    }
}