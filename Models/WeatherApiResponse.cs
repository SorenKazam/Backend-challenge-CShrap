using System.Text.Json.Serialization;

namespace desafioVisualFormaBackEnd.Models
{
    public class WeatherApiResponse
    {
        [JsonPropertyName("current_weather")]
        public CurrentWeather? CurrentWeather { get; set; }
    }

    public class CurrentWeather
    {
        [JsonPropertyName("temperature")]
        public double Temperature { get; set; }

        [JsonPropertyName("windspeed")]
        public double WindSpeed { get; set; }

        [JsonPropertyName("weathercode")]
        public double WeatherCode { get; set; }
    }
}
