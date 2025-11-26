using desafioVisualFormaBackEnd.Models;

namespace desafioVisualFormaBackEnd.Services
{
    public interface IWeatherService
    {
        Task<WeatherResult?> GetWeatherAsync(string cityName);
    }
}
