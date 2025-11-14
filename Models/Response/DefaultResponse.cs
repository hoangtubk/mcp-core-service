namespace mcp_core_service.Models.Response
{
    public class DefaultResponse
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public DefaultResponse(string message, int statusCode)
        {
            Message = message;
            StatusCode = statusCode;
        }
    }
}
