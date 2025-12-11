namespace mcp_core_service.Models.DTO
{
    public class TuyaDeviceDetail
    {
        public TuyaDeviceResult? result { get; set; }
        public bool success { get; set; }
        public long t { get; set; }
        public string? tid { get; set; }
    }

    public class TuyaDeviceStatus
    {
        public string? code { get; set; }
        public object? value { get; set; }
    }
}

