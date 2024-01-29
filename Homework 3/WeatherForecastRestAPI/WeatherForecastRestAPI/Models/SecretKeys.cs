namespace WeatherForecastRestAPI.Models;

public class SecretKey
{
    private readonly string _name;
    
    public SecretKey(string name)
    {
        _name = name;
    }

    public string GetFromEnvironment()
    {
        return Environment.GetEnvironmentVariable(_name) ?? 
               throw new InvalidOperationException($"Secret key {_name} was not found in environment variables");
    }
}

public static class SecretKeys
{
    public static readonly SecretKey TomorrowIoSecretKey = new("TIO_SECRET_KEY");
    public static readonly SecretKey OpenWeatherMapSecretKey = new("OWM_SECRET_KEY");
}