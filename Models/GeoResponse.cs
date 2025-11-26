using System.Text.Json.Serialization;

namespace desafioVisualFormaBackEnd.Models
{
    public class GeoResponse
    {
        [JsonPropertyName("results")]
        public List<CityResult>? Results { get; set; }

        [JsonPropertyName("generationtime_ms")]
        public double? GenerationTimeMs { get; set; }
    }

    public class GeoCityResult
    {
        [JsonPropertyName("id")]
        public long? Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;

        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }

        [JsonPropertyName("country_code")]
        public string? CountryCode { get; set; }

        [JsonPropertyName("country")]
        public string? Country { get; set; }

        [JsonPropertyName("timezone")]
        public string? Timezone { get; set; }
    }
}
