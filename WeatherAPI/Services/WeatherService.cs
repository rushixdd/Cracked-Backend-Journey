using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using WeatherAPI.Models;
using WeatherAPI.Services;

public class WeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    private readonly RedisService _redis;

    public WeatherService(HttpClient httpClient, IConfiguration config, RedisService redis)
    {
        _httpClient = httpClient;
        _config = config;
        _redis = redis;
    }

    public async Task<WeatherModel> GetWeatherAsync(string city)
    {
        // Check cache
        var cached = await _redis.GetCachedDataAsync(city);
        if (!string.IsNullOrEmpty(cached))
        {
            return JsonSerializer.Deserialize<WeatherModel>(cached)!;
        }

        // Fetch from API
        var baseUrl = _config["WeatherApi:BaseUrl"];
        var apiKey = _config["WeatherApi:ApiKey"];
        var url = $"{baseUrl}/{Uri.EscapeDataString(city)}?unitGroup=metric&key={apiKey}&contentType=json";

        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
            throw new Exception("Failed to fetch weather data.");

        var content = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(content);
        var root = doc.RootElement;

        var model = new WeatherModel
        {
            City = city,
            Conditions = root.GetProperty("currentConditions").GetProperty("conditions").GetString() ?? "Unknown",
            TemperatureC = root.GetProperty("currentConditions").GetProperty("temp").GetDouble(),
            RetrievedAt = DateTime.UtcNow
        };

        // Cache the serialized model
        var serialized = JsonSerializer.Serialize(model);
        await _redis.SetCachedDataAsync(city, serialized, TimeSpan.FromHours(12));

        return model;
    }
}
