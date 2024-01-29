using System.Net;

namespace WeatherForecastRestAPI.Models.WeatherForecast;

public record WeatherForecastServiceResponse
(
    HttpStatusCode StatusCode,
    string? Message
);