using mcp_core_service.Models.Response;
using mcp_core_service.Models.TuyaRequest;
using System.Text.Json;

namespace mcp_core_service.Services.Interface
{
    public interface ITuyaService
    {
        string Encrypt(string str, string secret);
        //Task<JsonElement> SendAsync(string method, string path, string body);
        Task<JsonElement> GetAsync(string path);
        Task PostAsync(string path, string bodyJson);
        Task<DeviceListResponse> GetAllDevices();
        Task<DeviceDetailResponse> GetDeviceById(string deviceId);
        Task<DefaultResponse> SendCommand(string deviceId, List<TuyaDeviceCommand> commands);
    }
}
