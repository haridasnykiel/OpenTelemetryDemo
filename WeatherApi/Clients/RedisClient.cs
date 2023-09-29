using StackExchange.Redis;

namespace WeatherApi.Clients;

public class RedisClient : IRedisClient
{
    private static ConnectionMultiplexer _connectionMultiplexer;
    public RedisClient(string redisHost)
    {
        if(_connectionMultiplexer == null) 
        {
            _connectionMultiplexer = ConnectionMultiplexer.Connect(redisHost);
        }  
    }

    public IDatabase GetDatabase()
    {
        return _connectionMultiplexer.GetDatabase();
    }

    public bool Set(IDatabase database, string key, string value)
    {
        return database.StringSet(key, value);
    }

    public string? Get(IDatabase database, string key) 
    {
        var value = database.StringGet(key);
        
        return value.HasValue ? value.ToString() : null;
    }
}