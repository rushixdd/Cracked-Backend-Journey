using WeatherAPI.Models;

namespace WeatherAPI.Services
{
    public interface IWeatherService
    {
        Task<WeatherModel?> GetWeatherAsync(string city);
    }
}
