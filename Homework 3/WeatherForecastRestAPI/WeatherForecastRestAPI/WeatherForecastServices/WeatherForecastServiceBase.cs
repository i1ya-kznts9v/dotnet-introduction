using System.Net;
using Newtonsoft.Json.Linq;
using WeatherForecastRestAPI.Models;
using WeatherForecastRestAPI.Models.WeatherForecast;

namespace WeatherForecastRestAPI.WeatherForecastServices;

public abstract class WeatherForecastServiceBase
{
    private readonly HttpClient _serviceClient = new();
    protected readonly SecretKey SecretKey;
    protected readonly Location Location;

    protected WeatherForecastServiceBase(
        WeatherForecastServiceInfo serviceInfo, 
        SecretKey secretKey, Location location)
    {
        SecretKey = secretKey;
        Location = location;
        ServiceInfo = serviceInfo;
    }
    
    public WeatherForecastServiceInfo ServiceInfo { get; }
    
    protected abstract string GetQueryParameters();

    private string GetFullQuery()
    {
        return new UriBuilder(ServiceInfo.Uri) { Query = GetQueryParameters() }.Uri.ToString();
    }
    
    protected abstract WeatherForecastInfo TryParseResponseJson(JObject jResponseContent);

    public WeatherForecast GetWeatherForecast()
    {
        HttpResponseMessage response;
        try
        {
            response = _serviceClient.GetAsync(GetFullQuery()).Result;
        }
        catch (Exception)
        {
            return WeatherForecast.Error(
                ServiceInfo,
                HttpStatusCode.ServiceUnavailable, 
                "No response received from service, try again later");
        }
        
        if (!response.IsSuccessStatusCode)
        {
            return WeatherForecast.Error(
                ServiceInfo,
                response.StatusCode,
                response.ReasonPhrase);
        }
        
        WeatherForecastInfo info;
        try
        {
            JObject jResponseContent = JObject.Parse(
                response.Content.ReadAsStringAsync().Result);
            info = TryParseResponseJson(jResponseContent);
        }
        catch (Exception)
        {
            return WeatherForecast.Error(
                ServiceInfo,
                response.StatusCode,
                "Invalid content received from service, try again later");
        }

        return WeatherForecast.Ok(
            info,
            ServiceInfo, 
            response.StatusCode, 
            response.ReasonPhrase);
    }
}