using StackExchange.Redis;

namespace Name;

public interface IRedisClient
{
    IDatabase GetDatabase();
    bool Set(IDatabase database, string key, string value);
    string? Get(IDatabase database, string key);
}