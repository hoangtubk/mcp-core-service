using mcp_core_service.Models.Response;
using mcp_core_service.Models.TuyaRequest;
using mcp_core_service.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace mcp_core_service.Controllers
{
    [Route("iotcore/v1.0")]
    [Authorize(AuthenticationSchemes = "BasicAuth")]
    [ApiController]
    public class IoTController : ControllerBase
    {
        private readonly IIoTCoreService _ioTCoreService;
        public IoTController(IIoTCoreService ioTCoreService)
        {
            _ioTCoreService = ioTCoreService;
        }

        [HttpGet("get_devices")]
        public async Task<DeviceListResponse> GetDevices()
        {
            DeviceListResponse response = await _ioTCoreService.GetAllDevices();
            return response;
        }

        [HttpGet("get_device_by_id/{id}")]
        public async Task<DeviceDetailResponse> GetDeviceById([FromRoute] string id)
        {
            DeviceDetailResponse response = await _ioTCoreService.GetDeviceById(id);
            return response;
        }

        [HttpPost("control_device/{id}/command")]
        public async Task<DefaultResponse> ControlDevice([FromRoute] string id, [FromBody] List<TuyaDeviceCommand> commands)
        {
            DefaultResponse response = await _ioTCoreService.ControlDevice(id, commands);
            return response;
        }
    }
}
