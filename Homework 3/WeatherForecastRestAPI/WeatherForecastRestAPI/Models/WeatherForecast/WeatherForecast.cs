using System.Net;

namespace WeatherForecastRestAPI.Models.WeatherForecast;

public record WeatherForecast
{
    public WeatherForecastInfo? Info { get; init; }
    public WeatherForecastServiceInfo? ServiceInfo { get; init; }
    public WeatherForecastServiceResponse? ServiceResponse { get; init; }
    
    public static WeatherForecast Ok(
        WeatherForecastInfo info,
        WeatherForecastServiceInfo serviceInfo, 
        HttpStatusCode statusCode, 
        string? message)
    {
        return new WeatherForecast
        {
            Info = info,
            ServiceInfo = serviceInfo,
            ServiceResponse = new WeatherForecastServiceResponse(
                statusCode, message)
        };
    }

    public static WeatherForecast Error(
        WeatherForecastServiceInfo serviceInfo, 
        HttpStatusCode statusCode, 
        string? message)
    {
        return new WeatherForecast
        {
            ServiceInfo = serviceInfo,
            ServiceResponse = new WeatherForecastServiceResponse(
                statusCode, message)
        };
    }
}