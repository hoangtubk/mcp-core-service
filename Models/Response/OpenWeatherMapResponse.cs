namespace mcp_core_service.Models.Response
{
    public class OpenWeatherMapResponse : DefaultResponse
    {
        public OpenWeatherMap result { get; set; }
        public OpenWeatherMapResponse(string message, int statusCode, OpenWeatherMap openWeatherMap)
            : base(message, statusCode)
        {
            result = openWeatherMap;
        }
    }

    public class OpenWeatherMap
    {
        public float lat { get; set; }
        public float lon { get; set; }
        public string? tz { get; set; }
        public string? date { get; set; }
        public CloudCover? cloud_cover { get; set; }
        public Precipitation? precipitation { get; set; }
        public Temperature? temperature { get; set; }
        public Pressure? pressure { get; set; }
        public Wind? wind { get; set; }
    }

    public class Wind
    {
        public Max? max { get; set; }
    }

    public class Max
    {
        public float speed { get; set; }
        public float direction { get; set; }
    }

    public class Pressure
    {
        public float afternoon { get; set; }
    }

    public class Temperature
    {
        public float min { get; set; }
        public float max { get; set; }
        public float afternoon { get; set; }
        public float night { get; set; }
        public float evening { get; set; }
        public float morning { get; set; }
    }

    public class Precipitation
    {
        public float total { get; set; }
    }

    public class Humidity
    {
        public float afternoon { get; set; }
    }

    public class CloudCover
    {
        public float afternoon { get; set; }
    }
}
