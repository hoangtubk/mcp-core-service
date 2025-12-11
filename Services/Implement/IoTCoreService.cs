using mcp_core_service.Models.Response;
using mcp_core_service.Models.TuyaRequest;
using mcp_core_service.Services.Interface;

namespace mcp_core_service.Services.Implement
{
    
    public class IoTCoreService : IIoTCoreService
    {
        private readonly ITuyaService _tuyaService;
        public IoTCoreService(IMCPCoreService mCPCoreService, ITuyaService tuyaService)
        {
            _tuyaService = tuyaService;
        }
        
        public async Task<DefaultResponse> ControlDevice(string device, List<TuyaDeviceCommand> commands)
        {
            DefaultResponse response = await _tuyaService.SendCommand(device, commands);
            return response;
        }

        public async Task<DeviceDetailResponse> GetDeviceById(string id)
        {
            DeviceDetailResponse response = await _tuyaService.GetDeviceById(id);
            return response;
        }

        public async Task<DeviceListResponse> GetAllDevices()
        {
            DeviceListResponse response = await _tuyaService.GetAllDevices();
            return response;
        }
    }
}
