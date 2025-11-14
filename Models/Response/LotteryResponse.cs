namespace mcp_core_service.Models.Response
{
    public class LotteryResponse : DefaultResponse
    {
        public class LotteryResult
        {
            public required string region { get; set; }
            public required string lotteryTitle { get; set; }
            public required string lotteryDesc { get; set; }
            public required string lotteryDate { get; set; }
            public required List<LotteryDetail> lotteryDetails { get; set; }
        }
        public LotteryResult Result { get; set; }
        public LotteryResponse(string message, int statusCode, LotteryResult lottery)
            : base(message, statusCode)
        {
            Result = lottery;
        }
    }

    public class LotteryDetail
    {
        public required string key { get; set; }
        public required List<string> numbers { get; set; }
    }
}
