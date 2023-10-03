using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WeatherApi.Services;

namespace WeatherApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    IWeatherForecastService service;

    public WeatherForecastController(IWeatherForecastService service)
    {
        this.service = service;
    }

    [HttpGet(Name = "GetWeather")]
    public IActionResult Get([FromQuery] List<DateOnly> dates)
    {
        var forecasts = service.GetForecasts(dates);

        if(forecasts.Count is 0) {
            return NoContent();
        }

        return Ok(forecasts);
    }

    [HttpPost(Name = "PostWeather")]
    public IActionResult Post(IEnumerable<WeatherForecast> weatherForecasts) 
    {
        var validForecasts = weatherForecasts
        .Where(w => w.Date > DateOnly.FromDateTime(DateTime.UtcNow))
        .ToList();

        if(validForecasts.Count <= 0)
        {
            return BadRequest();
        }

        service.AddForecasts(validForecasts);

        return Ok();
    }   
}
