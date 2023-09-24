using Microsoft.AspNetCore.Mvc;

namespace WeatherApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {

        using var activity = DiagnosticsConfig.ActivitySource.StartActivity("Get Weather Forecasts");


        // DiagnosticsConfig.RequestCounter.Add(1,
        //     new("Action", "GetWeatherForecast"),
        //     new("Controller", nameof(WeatherForecastController)));

        var forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();

        // create a constants class to include all the keys.
        activity?.SetTag("forecasts.count", forecasts.Length);
        activity?.SetTag("forecasts.first", forecasts[0].Summary);

        return forecasts;
    }
}
