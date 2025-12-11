namespace mcp_core_service.Models.Response
{
    public class DeviceDetailResponse : DefaultResponse
    {
        public class DeviceDetailResult
        {
            public required DeviceDetail device { get; set; }
        }
        public DeviceDetailResult Result { get; set; }
        public DeviceDetailResponse(string message, int statusCode, DeviceDetailResult device)
            : base(message, statusCode)
        {
            Result = device;
        }
    }
}
