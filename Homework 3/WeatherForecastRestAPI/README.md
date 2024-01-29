# Weather Forecast REST API Web Service

## API routes available

`/weather/services` - get available weather forecast services

`/weather/forecast?service=` - get weather forecasts from all available services 
(`service` == TomorrowIO | `service` == OpenWeatherMap)

`/weather/forecast/all` - get weather forecast from the specified service

## Docker support

First, **create .env file in the solution root directory (./WeatherForecastRestAPI)**

Initialize secret keys for *TomorrowIO* and *OpenWeatherMap* services in *.env* file:

```dotenv
TIO_SECRET_KEY=[your secret key]
OWM_SECRET_KEY=[your secret key]
```

### Build Docker image
```shell
docker build -t weather-forecast -f \Dockerfile .
```

### Run Docker image
```shell
docker run -it --rm -p 5000:80 --env-file=./.env  weather-forecast
```

Open `http://localhost:5000` in a browser and test the API.

### Run ASP.NET Core development mode and SwaggerUI
```shell
docker run -it --rm -p 5000:80 -e ASPNETCORE_ENVIRONMENT=Development --env-file=./.env  weather-forecast
```

In development version you can use Swagger UI.
Go to `http://localhost:5000/swagger/index.html` to open app with Swagger UI.