using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WeatherForecastRestAPI.Models;
using WeatherForecastRestAPI.Models.WeatherForecast;

namespace WeatherForecastRestAPI.WeatherForecastServices;

public class TomorrowIoService : WeatherForecastServiceBase
{
    public TomorrowIoService(Location location) : base(new WeatherForecastServiceInfo(
        "TomorrowIO", new Uri("https://api.tomorrow.io/v4/timelines")),
        SecretKeys.TomorrowIoSecretKey, location)
    { }

    protected override string GetQueryParameters()
    {
        return $"location={Location.Latitude}," +
               $"{Location.Longitude}" +
               "&fields=temperature" +
               "&fields=cloudCover" +
               "&fields=humidity" +
               "&fields=precipitationProbability" +
               "&fields=windDirection" +
               "&fields=windSpeed" +
               "&units=metric" +
               "&timesteps=current" +
               "&startTime=now" +
               "&endTime=nowPlus1m" +
               "&timezone=auto" +
               $"&apikey={SecretKey.GetFromEnvironment()}";
    }

    protected override WeatherForecastInfo TryParseResponseJson(JObject jResponseContent)
    {
        JToken? valuesToken = jResponseContent.SelectToken("data.timelines[0].intervals[0].values");

        if (valuesToken == null)
        {
            throw new JsonException("Invalid JSON received in service response");
        }

        return new WeatherForecastInfo
        {
            DateTime = DateTime.Now,
            TemperatureC = valuesToken.SelectToken("temperature")?.ToObject<string>() ??
                           WeatherForecastInfo.NoContentAvailable(),
            Cloudiness = valuesToken.SelectToken("cloudCover")?.ToObject<string>() ??
                         WeatherForecastInfo.NoContentAvailable(),
            Humidity = valuesToken.SelectToken("humidity")?.ToObject<string>() ??
                       WeatherForecastInfo.NoContentAvailable(),
            Precipitation = valuesToken.SelectToken("precipitationProbability")?.ToObject<string>() ??
                                       WeatherForecastInfo.NoContentAvailable(),
            WindDirection = valuesToken.SelectToken("windDirection")?.ToObject<string>() ??
                            WeatherForecastInfo.NoContentAvailable(),
            WindSpeed = valuesToken.SelectToken("windSpeed")?.ToObject<string>() ??
                        WeatherForecastInfo.NoContentAvailable()
        };
    }
}