namespace mcp_core_service.Models.Response
{
    public class DeviceListResponse : DefaultResponse
    {
        public class DeviceListResult
        {
            public required List<DeviceDetail> devices { get; set; }
        }
        public DeviceListResult Result { get; set; }
        public DeviceListResponse(string message, int statusCode, DeviceListResult devices)
            : base(message, statusCode)
        {
            Result = devices;
        }
    }
}
