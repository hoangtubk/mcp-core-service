namespace mcp_core_service.Models.Response
{
    public class CoinPriceResponse : DefaultResponse
    {
        public class CoinPriceResult
        {
            public required string CoinId { get; set; }
            public required string Currency { get; set; }
            public required string Amount { get; set; }
        }
        public CoinPriceResult Result { get; set; }
        public CoinPriceResponse(string message, int statusCode, CoinPriceResult coinPrice)
            : base(message, statusCode)
        {
            Result = coinPrice;
        }
    }
}
