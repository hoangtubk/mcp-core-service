namespace mcp_core_service.Models.Response
{
    public class AuPriceResponse : DefaultResponse
    {
        public List<AuPrice> results { get; set; }
        public AuPriceResponse(string message, int statusCode, List<AuPrice> auPrice) 
            : base(message, statusCode)
        {
            results = auPrice;
        }
    }

    public class AuPrice
    {
        public string? trademark { get; set; }
        public List<ProfilePrice>? profile { get; set; }
    }

    public class ProfilePrice
    {
        public string? type { get; set; }
        public string? desc { get; set; }
        public string? bid { get; set; }
        public string? ask { get; set; }
        public string? unit { get; set; }
        public DateTime? updated_at { get; set; }
    }
}
