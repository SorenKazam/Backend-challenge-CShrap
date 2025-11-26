namespace desafioVisualFormaBackEnd.Models
{
    public class WeatherResult
    {
        public double Temperature { get; set; }
        public string Condition { get; set; } = null!;
        public double? WindSpeed { get; set; }
        public bool FromCache { get; set; } = false;
    }
}
