using Microsoft.AspNetCore.Mvc;
using WeatherForecastRestAPI.Models;
using WeatherForecastRestAPI.Models.WeatherForecast;
using WeatherForecastRestAPI.WeatherForecastServices;

namespace WeatherForecastRestAPI.Controllers;

[ApiController]
[Route("weather")]
public class WeatherForecastRestApiController : ControllerBase
{
    private readonly Dictionary<string, WeatherForecastServiceBase> _services = new();

    public WeatherForecastRestApiController()
    {
        List<WeatherForecastServiceBase> services = new()
        {
            new TomorrowIoService(Locations.SaintPetersburgLocation),
            new OpenWeatherMapService(Locations.SaintPetersburgLocation)
        };
        foreach (WeatherForecastServiceBase service in services)
        {
            _services.Add(service.ServiceInfo.Name, service);
        }
    }
    
    /// <summary>
    /// Get available weather forecast services
    /// </summary>
    /// <returns>
    /// List with information about available services
    /// </returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("services")]
    public IEnumerable<WeatherForecastServiceInfo> GetServices()
    {
        return _services.Select(serviceEntry=> serviceEntry.Value.ServiceInfo);
    }
    
    /// <summary>
    /// Get weather forecast from the specified service
    /// </summary>
    /// <param name="service">
    /// Name of the service
    /// </param>
    /// <returns>
    /// Weather forecast from the specified service
    /// </returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("forecast")]
    public ActionResult<WeatherForecast> GetServiceForecast([FromQuery] string service)
    {
        if (!_services.ContainsKey(service))
        {
            return NotFound($"Service {service} is not available");
        }
        return _services[service].GetWeatherForecast();
    }
    
    /// <summary>
    /// Get weather forecasts from all available services
    /// </summary>
    /// <returns>
    /// List with weather forecasts from all available services
    /// </returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("forecast/all")]
    public IEnumerable<WeatherForecast> GetAllServicesForecast()
    {
        return _services.Select(serviceEntry => serviceEntry.Value.GetWeatherForecast());
    }
}