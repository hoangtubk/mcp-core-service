namespace mcp_core_service.Helpper
{
    public class TuyaOptions
    {
        public const string SectionName = "TuyaOptions";
        public required string ClientId { get; set; }
        public required string Secret { get; set; }
        public required string ApiUrl { get; set; }
    }
}
