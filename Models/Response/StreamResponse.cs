using ZingMP3Explode.Entities;

namespace mcp_core_service.Models.Response
{
    public class StreamResponse : DefaultResponse
    {
        public class StreamResult
        {
            public required string Id { get; set; }
            public required string StreamUrl { get; set; }
            public required string Title { get; set; }
            public required string Artist { get; set; }
            public required string FullUrl { get; set; }
            public long Duration { get; set; }
            public required string LyricUrl { get; set; }
        }
        public StreamResult result { get; set; }

        public StreamResponse(string message, int statusCode, StreamResult song)
            : base(message, statusCode)
        {
            result = song;
        }
    }
}
