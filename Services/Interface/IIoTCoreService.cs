using mcp_core_service.Models.Response;
using mcp_core_service.Models.TuyaRequest;

namespace mcp_core_service.Services.Interface
{
    public interface IIoTCoreService
    {
        Task<DeviceListResponse> GetAllDevices();
        Task<DeviceDetailResponse> GetDeviceById(string id);
        Task<DefaultResponse> ControlDevice(string device, List<TuyaDeviceCommand> commands);
    }
}
