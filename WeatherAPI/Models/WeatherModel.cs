namespace WeatherAPI.Models
{
    public class WeatherModel
    {
        public string City { get; set; } = string.Empty;
        public string Conditions { get; set; } = string.Empty;
        public double TemperatureC { get; set; }
        public double TemperatureF => Math.Round((TemperatureC * 9 / 5) + 32, 2);
        public DateTime RetrievedAt { get; set; }

    }
}
