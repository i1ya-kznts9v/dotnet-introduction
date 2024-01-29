namespace WeatherForecastRestAPI.Models;

public record Location(string Latitude, string Longitude);

public static class Locations
{
    public static readonly Location SaintPetersburgLocation = new("59.921101", "30.343123");
}