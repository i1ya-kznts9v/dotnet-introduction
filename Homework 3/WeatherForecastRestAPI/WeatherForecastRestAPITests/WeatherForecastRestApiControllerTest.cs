using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using NUnit.Framework;
using WeatherForecastRestAPI.Controllers;
using WeatherForecastRestAPI.Models.WeatherForecast;

namespace WeatherForecastRestAPITests;

public class WeatherForecastRestApiControllerTest
{
    private readonly WeatherForecastRestApiController _controller = new();
    private readonly List<WeatherForecastServiceInfo> _expectedServices = new()
    {
        new WeatherForecastServiceInfo("TomorrowIO", new Uri("https://api.tomorrow.io/v4/timelines")),
        new WeatherForecastServiceInfo("OpenWeatherMap", new Uri("https://api.openweathermap.org/data/2.5/weather"))
    };

    [SetUp]
    public void Setup()
    {
        var envPath = Directory.GetCurrentDirectory() + "/../../../../.env";
        SetEnvironmentVariables(envPath);
    }

    [Test]
    public void GetServicesTest()
    {
        var actualServices = _controller.GetServices();
        Assert.That(_expectedServices.SequenceEqual(actualServices));
    }
    
    [Test]
    public void GetServiceForecastTest()
    {
        foreach (var expectedService in _expectedServices)
        {
            var actualForecast = _controller.GetServiceForecast(expectedService.Name);
            
            Assert.That(_expectedServices.Any(service => service.Name == actualForecast.Value!.ServiceInfo!.Name));
            Assert.That(_expectedServices.Any(service => service.Uri == actualForecast.Value!.ServiceInfo!.Uri));
            
            Assert.That(HttpStatusCode.OK == actualForecast.Value!.ServiceResponse!.StatusCode);
            Assert.That("OK" == actualForecast.Value!.ServiceResponse!.Message);
        }
    }
    
    [Test]
    public void GetAllServicesForecastTest()
    {
        var actualForecasts = _controller.GetAllServicesForecast();
        foreach (var actualForecast in actualForecasts)
        {
            Assert.That(_expectedServices.Any(service => service.Name == actualForecast.ServiceInfo!.Name));
            Assert.That(_expectedServices.Any(service => service.Uri == actualForecast.ServiceInfo!.Uri));
        
            Assert.That(HttpStatusCode.OK == actualForecast.ServiceResponse!.StatusCode);
            Assert.That("OK" == actualForecast.ServiceResponse!.Message);
        }
    }

    private static void SetEnvironmentVariables(string path)
    {
        if (!File.Exists(path)) return;
        foreach (var line in File.ReadAllLines(path))
        {
            var entry = line.Split('=', StringSplitOptions.RemoveEmptyEntries);
            if (entry.Length != 2) continue;
            Environment.SetEnvironmentVariable(entry[0], entry[1]);
        }
    }
}