namespace mcp_core_service.Models.DTO
{
    public class TuyaDeviceResult
    {
        public long active_time { get; set; }
        public int biz_type { get; set; }
        public string? category { get; set; }
        public long create_time { get; set; }
        public string? icon { get; set; }
        public string? id { get; set; }
        public string? ip { get; set; }
        public string? lat { get; set; }
        public string? local_key { get; set; }
        public string? lon { get; set; }
        public string? model { get; set; }
        public string? name { get; set; }
        public bool online { get; set; }
        public string? owner_id { get; set; }
        public string? product_id { get; set; }
        public string? product_name { get; set; }
        public List<TuyaDeviceStatus>? status { get; set; }
    }
}
