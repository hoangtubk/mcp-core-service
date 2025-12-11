namespace mcp_core_service.Models.DTO
{
    public class TuyaCommand
    {
        public bool result { get; set; }
        public bool success { get; set; }
        public long t { get; set; }
        public string? tid { get; set; }

    }
}
