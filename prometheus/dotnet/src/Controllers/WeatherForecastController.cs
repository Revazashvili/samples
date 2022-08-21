using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Prometheus;

namespace dotnet.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly Counter ProcessedJobCount = Metrics.CreateCounter("dotnet_request_operations_total", "The total number of processed requests");
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
        var sw = Stopwatch.StartNew();
        var result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();

        sw.Stop();
        
        ProcessedJobCount.Inc();
        var histogram = Metrics.CreateHistogram(
                "dotnet_request_duration_seconds",
                "Histogram for the duration in seconds.",
                new HistogramConfiguration
                {
                    Buckets = Histogram.LinearBuckets(start: 1, width: 1, count: 5)
                });

        histogram.Observe(sw.Elapsed.TotalSeconds);
        return result;
    }
}
