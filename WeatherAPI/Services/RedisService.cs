using StackExchange.Redis;
using System.Numerics;

public class RedisService
{
    private readonly IDatabase _db;

    public RedisService(IConfiguration config)
    {
        var redis = ConnectionMultiplexer.Connect(new ConfigurationOptions
        {
            EndPoints = { { config["Redis:Endpoint"], int.Parse(config["Redis:Port"]) } },
            User = config["Redis:UserName"],
            Password = config["Redis:Password"]
        }
        );
        _db = redis.GetDatabase();
    }

    public async Task<string?> GetCachedDataAsync(string key) =>
        await _db.StringGetAsync(key);

    public async Task SetCachedDataAsync(string key, string value, TimeSpan? expiry = null) =>
        await _db.StringSetAsync(key, value, expiry ?? TimeSpan.FromHours(12));
}
