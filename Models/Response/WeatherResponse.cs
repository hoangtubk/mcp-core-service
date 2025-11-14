namespace mcp_core_service.Models.Response
{
    public class WeatherResponse : DefaultResponse
    {
        public class WeatherResult
        {
            public required string Location { get; set; }
            public required string Description { get; set; }
            public float TemperatureCelsius { get; set; }
            public float Humidity { get; set; }
            public float WindSpeedKph { get; set; }
        }
        public WeatherResult Weather { get; set; }
        public WeatherResponse(string message, int statusCode, WeatherResult weather)
            : base(message, statusCode)
        {
            Weather = weather;
        }
    }
}
