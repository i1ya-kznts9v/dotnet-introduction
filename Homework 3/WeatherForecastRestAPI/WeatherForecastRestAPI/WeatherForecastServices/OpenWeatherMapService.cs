using Newtonsoft.Json.Linq;
using WeatherForecastRestAPI.Models;
using WeatherForecastRestAPI.Models.WeatherForecast;

namespace WeatherForecastRestAPI.WeatherForecastServices;

public class OpenWeatherMapService : WeatherForecastServiceBase
{
    public OpenWeatherMapService(Location location) : base(new WeatherForecastServiceInfo(
        "OpenWeatherMap", new Uri("https://api.openweathermap.org/data/2.5/weather")),
        SecretKeys.OpenWeatherMapSecretKey, location)
    { }

    protected override string GetQueryParameters()
    {
        return $"lat={Location.Latitude}" +
               $"&lon={Location.Longitude}" +
               "&units=metric" +
               $"&appid={SecretKey.GetFromEnvironment()}";
    }

    protected override WeatherForecastInfo TryParseResponseJson(JObject jObject)
    {
        return new WeatherForecastInfo
        {
            DateTime = DateTime.Now,
            TemperatureC = jObject.SelectToken("main.temp")?.ToObject<string>() ??
                           WeatherForecastInfo.NoContentAvailable(),
            Cloudiness = jObject.SelectToken("clouds.all")?.ToObject<string>() ??
                         WeatherForecastInfo.NoContentAvailable(),
            Humidity = jObject.SelectToken("main.humidity")?.ToObject<string>() ??
                       WeatherForecastInfo.NoContentAvailable(),
            Precipitation = jObject.SelectToken("weather[0].main")?.ToObject<string>() ??
                            WeatherForecastInfo.NoContentAvailable(),
            WindDirection = jObject.SelectToken("wind.deg")?.ToObject<string>() ??
                            WeatherForecastInfo.NoContentAvailable(),
            WindSpeed = jObject.SelectToken("wind.speed")?.ToObject<string>() ??
                        WeatherForecastInfo.NoContentAvailable()
        };
    }
}