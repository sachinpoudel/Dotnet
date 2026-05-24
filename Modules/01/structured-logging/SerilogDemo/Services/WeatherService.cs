namespace SerilogDemo.Services;
public record WeatherForecast
{
    public DateOnly Date { get; init;}
    public int TemperatureC {get; init;}

    public string? Summary {get; init;}

    public int TemperatureF => 32 + (int)(TemperatureC / 0.556);
}
public interface IWeatherService
{
    IEnumerable<WeatherForecast> GetForecast();
}

public class WeatherService(ILogger<WeatherService> logger) : IWeatherService
{
    private static readonly string[] Summaries =
        ["Freezing", "Cool", "Mild", "Warm", "Hot", "Scorching"];

    public IEnumerable<WeatherForecast> GetForecast()
    {
        logger.LogInformation("Generating weather forecast for {Days} days", 5);

        var forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        }).ToList();

        logger.LogInformation("Generated {Count} forecasts, average temp: {AvgTemp}°C",
            forecasts.Count, forecasts.Average(f => f.TemperatureC));

        return forecasts;
    }
}