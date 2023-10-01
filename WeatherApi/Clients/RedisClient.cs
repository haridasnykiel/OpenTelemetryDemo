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

    public IDatabase GetDatabase() => _connectionMultiplexer.GetDatabase();

    public bool Set(IDatabase database, string key, string value)
    {
        try 
        {
            return database.StringSet(key, value);
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
        
    }

    public string? Get(IDatabase database, string key) 
    {
        try 
        {
            var value = database.StringGet(key);
            
            if(!value.HasValue)
            {
                Console.WriteLine("Value not found");
            }
        
            return value.HasValue ? value.ToString() : null;
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            return string.Empty;
        }

        
    }
}