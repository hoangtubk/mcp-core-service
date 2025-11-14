namespace mcp_core_service.Models.Response
{
    public class MusicResult
    {
        public required string Id { get; set; }
        public required string Title { get; set; }
        public required string Artist { get; set; }
        public required string FullUrl { get; set; }
        public long Duration { get; set; }
    }

    public class MusicResponse : DefaultResponse
    {
        public List<MusicResult> results { get; set; }

        public MusicResponse(string message, int statusCode, List<MusicResult> songs) 
            : base(message, statusCode)
        {
            results = songs;
        }
    }
}
