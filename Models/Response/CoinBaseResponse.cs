namespace mcp_core_service.Models.Response
{
    public class CoinBaseResponse
    {
        public CoinBaseData? data { get; set; }
    }

    public class CoinBaseData
    {
        public string? amount { get; set; }
        public string? @base { get; set; }
        public string? currency { get; set; }
    }
}
