namespace mcp_core_service.Models.DTO
{
    public class TuyaDeviceList
    {
        public TuyaDeviceListResult? result { get; set; }
        public bool success { get; set; }
        public long t { get; set; }
        public string? tid { get; set; }
    }

    public class TuyaDeviceListResult
    {
        public List<TuyaDeviceResult>? devices { get; set; }
        public bool has_more { get; set; }
        public int total { get; set; }
    }
}
