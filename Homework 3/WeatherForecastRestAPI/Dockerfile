FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WeatherForecastRestAPI/WeatherForecastRestAPI.csproj", "WeatherForecastRestAPI/"]
RUN dotnet restore "WeatherForecastRestAPI/WeatherForecastRestAPI.csproj"
COPY . .
WORKDIR "/src/WeatherForecastRestAPI"
RUN dotnet build "WeatherForecastRestAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WeatherForecastRestAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WeatherForecastRestAPI.dll"]
