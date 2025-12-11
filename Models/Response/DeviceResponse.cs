using mcp_core_service.Models.DTO;

namespace mcp_core_service.Models.Response
{
    public class DeviceResponse: DefaultResponse
    {
        public class DeviceResult
        {
            public required List<DeviceDetail> devices { get; set; }
        }
        public DeviceResult Result { get; set; }
        public DeviceResponse(string message, int statusCode, DeviceResult device)
            : base(message, statusCode)
        {
            Result = device;
        }
    }

    public class DeviceDetail
    {
        public required string deviceId { get; set; }
        public required string deviceName { get; set; }
        public required string deviceType { get; set; }
        public required List<TuyaDeviceStatus> status { get; set; }
    }
}
