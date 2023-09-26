using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Name;

namespace WeatherApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    IRedisClient redisClient;

    public WeatherForecastController(IRedisClient redisClient)
    {
        this.redisClient = redisClient;
    }

    [HttpGet(Name = "GetWeather")]
    public IActionResult Get([FromQuery] List<DateOnly> Dates)
    {

        using var activity = DiagnosticsConfig.ActivitySource.StartActivity("Get Weather Forecasts");

        var redisDatabase = redisClient.GetDatabase();

        var forecasts = Dates.Select(date => {

            var key = date.ToString();
            var result = redisClient.Get(redisDatabase, date.ToString());
            
            if(result is null) 
            {
                activity?.SetTag("forecasts.date.notfound", date.ToString());
                return null;
            }
            
            return JsonSerializer.Deserialize<WeatherForecast>(result);
        })
        .ToArray();

        // create a constants class to include all the keys.
        activity?.SetTag("forecasts.found.count", forecasts.Count(f => f != null));

        return Ok(forecasts);
    }

    [HttpPost(Name = "PostWeather")]
    public void Post(IEnumerable<WeatherForecast> weatherForecasts) 
    {
        using var activity = DiagnosticsConfig.ActivitySource.StartActivity("Post Weather Forecasts");

        var validForecasts = weatherForecasts.Where(w => w.Date > DateOnly.FromDateTime(DateTime.UtcNow));

        var redisDatabase = redisClient.GetDatabase();

        foreach (var forecast in validForecasts)
        {
            var hasSet = redisClient.Set(redisDatabase, forecast.Date.ToString(), JsonSerializer.Serialize(forecast));

            var message = $"{forecast.Date} -> {forecast.Summary} {forecast.TemperatureC}c";
            
            if(hasSet)
            {
                activity?.SetTag("forecast.added", message);
            }
            else 
            {
                activity?.SetTag("forecast.added.failed", message);
            }
        }
    }   
}
