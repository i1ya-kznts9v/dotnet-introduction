using System;
using NUnit.Framework;
using WeatherForecastRestAPI.Models;

namespace WeatherForecastRestAPITests;

public class SecretKeyTest
{
    private const string ExpectedSecretKey = "expected-secret-key";
    
    [SetUp]
    public void Setup()
    {
        Environment.SetEnvironmentVariable("TIO_SECRET_KEY", ExpectedSecretKey);
        Environment.SetEnvironmentVariable("OWM_SECRET_KEY", ExpectedSecretKey);
    }

    [Test]
    public void GetSecretKeyFromEnvironmentVariableTest()
    {
        Assert.AreEqual(SecretKeys.TomorrowIoSecretKey.GetFromEnvironment(), ExpectedSecretKey);
        Assert.AreEqual(SecretKeys.OpenWeatherMapSecretKey.GetFromEnvironment(), ExpectedSecretKey);
    }
    
    [Test]
    public void FailsIfSecretKeyNotInEnvironmentVariableTest()
    {
        Assert.Throws<InvalidOperationException>(() => new SecretKey("SG_SECRET_KEY").GetFromEnvironment());
    }
}