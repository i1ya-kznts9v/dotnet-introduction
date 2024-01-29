namespace WeatherForecastRestAPI.Models.WeatherForecast;

public record WeatherForecastInfo
{
    public DateTime DateTime { get; init; } = DateTime.Now;
    public string TemperatureC { get; init; } = NoContentAvailable();
    public string TemperatureF => ConvertCelsiusToFahrenheit(TemperatureC);
    public string Cloudiness { get; init; } = NoContentAvailable();
    public string Humidity { get; init; } = NoContentAvailable();
    public string Precipitation { get; init; } = NoContentAvailable();
    public string WindDirection { get; init; } = NoContentAvailable();
    public string WindSpeed { get; init; } = NoContentAvailable();
    
    private static string ConvertCelsiusToFahrenheit(string? temperatureC)
    {
        return double.TryParse(temperatureC, out double doubleTemperatureC)
            ? ((doubleTemperatureC + 32) / 0.5556).ToString("F")
            : NoContentAvailable();
    }

    public static string NoContentAvailable()
    {
        return "Content is not available at this moment";
    }
}