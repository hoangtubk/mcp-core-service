using ZingMP3Explode.Entities;

namespace mcp_core_service.Models.Response
{
    public class LyricResponse : DefaultResponse
    {
        public LyricData result { get; set; }

        public LyricResponse(string message, int statusCode, LyricData lyric)
            : base(message, statusCode)
        {
            result = lyric;
        }
    }
}
