using StackExchange.Redis;

namespace WeatherApi.Clients;

class RedisClient
{
    private static ConnectionMultiplexer _connectionMultiplexer;
    public RedisClient(string redisHost)
    {
        if(_connectionMultiplexer == null) 
        {
            _connectionMultiplexer = ConnectionMultiplexer.Connect(redisHost);
        }  
    }

    public static IDatabase GetDatabase()
    {
        return _connectionMultiplexer.GetDatabase();
    }

    public bool Set(IDatabase database, string key, string value)
    {
        return database.StringSet(key, value);
    }

    public string Get(IDatabase database, string key) {
        return database.StringGet(key).HasValue ? database.StringGet(key).ToString() : null;
    }
}