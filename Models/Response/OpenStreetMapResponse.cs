namespace mcp_core_service.Models.Response
{
    public class OpenStreetMapResponse : DefaultResponse
    {
        public OpenStreetMap result { get; set; }
        public OpenStreetMapResponse(string message, int statusCode, OpenStreetMap openStreetMap)
            : base(message, statusCode)
        {
            result = openStreetMap;
        }
    }

    public class OpenStreetMap
    {
        public string? place_id { get; set; }
        public string? licence { get; set; }
        public string? osm_type { get; set; }
        public long osm_id { get; set; }
        public float lat { get; set; }
        public float lon { get; set; }
        public string? @class { get; set; }
        public string? type { get; set; }
        public int place_rank { get; set; }
        public float importance { get; set; }
        public string? addresstype { get; set; }
        public string? name { get; set; }
        public string? display_name { get; set; }
        public List<string>? boundingbox { get; set; } = new List<string>();
    }
}
