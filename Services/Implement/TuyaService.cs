using mcp_core_service.Helpper;
using mcp_core_service.Middlewares;
using mcp_core_service.Models.DTO;
using mcp_core_service.Models.Response;
using mcp_core_service.Models.TuyaRequest;
using mcp_core_service.Services.Interface;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace mcp_core_service.Services.Implement
{
    public class TuyaService : ITuyaService
    {
        private readonly string _clientId;
        private readonly string _secret;
        private readonly HttpClient _http;
        private readonly TuyaOptions _options;

        private string _accessToken = string.Empty;
        private long _expireTime = 0;


        public TuyaService(IOptions<TuyaOptions> options)
        {
            _options = options.Value;
            _clientId = _options.ClientId;
            _secret = _options.Secret;
            _http = new HttpClient
            {
                BaseAddress = new Uri(_options.ApiUrl)
            };
        }

        public string Encrypt(string str, string secret)
        {
            secret = secret ?? "";
            var encoding = new UTF8Encoding();
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(str);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashmessage.Length; i++)
                {
                    builder.Append(hashmessage[i].ToString("x2"));
                }
                return builder.ToString().ToUpper();
            }
        }

        private static string GetTimestamp()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
        }

        private static string Sign(string str, string secret)
        {
            var encoding = new UTF8Encoding();
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(str);

            using var hmac = new HMACSHA256(keyByte);
            byte[] hash = hmac.ComputeHash(messageBytes);

            var sb = new StringBuilder();
            foreach (var b in hash)
                sb.Append(b.ToString("x2"));

            return sb.ToString().ToUpper();
        }

        private async Task EnsureAccessTokenAsync()
        {
            var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            if (string.IsNullOrEmpty(_accessToken) || now >= _expireTime - 60)
            {
                await GetTokenAsync();
            }
        }

        private async Task GetTokenAsync()
        {
            string path = "/v1.0/token?grant_type=1";

            string t = GetTimestamp();
            string nonce = Guid.NewGuid().ToString();

            string contentHash = Sha256("");
            string stringToSign = $"GET\n{contentHash}\n\n{path}";

            string sign = Sign(_clientId + t + nonce + stringToSign, _secret);

            var request = new HttpRequestMessage(HttpMethod.Get, path);
            request.Headers.Add("client_id", _clientId);
            request.Headers.Add("sign", sign);
            request.Headers.Add("t", t);
            request.Headers.Add("nonce", nonce);
            request.Headers.Add("sign_method", "HMAC-SHA256");

            var response = await _http.SendAsync(request);
            string json = await response.Content.ReadAsStringAsync();

            var doc = JsonDocument.Parse(json);

            if (!doc.RootElement.GetProperty("success").GetBoolean())
                throw new Exception($"GetToken failed: {json}");

            var result = doc.RootElement.GetProperty("result");

            _accessToken = result.GetProperty("access_token").GetString();
            _expireTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds() +
                          result.GetProperty("expire_time").GetInt64();
        }

        private static string Sha256(string input)
        {
            using var sha = SHA256.Create();
            byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));

            var sb = new StringBuilder();
            foreach (var b in bytes)
                sb.Append(b.ToString("x2"));

            return sb.ToString();
        }

        public async Task<JsonElement> SendAsync(string method, string path, string body, KeyValuePair<string, string>[] headers)
        {
            string t = GetTimestamp();
            string nonce = Guid.NewGuid().ToString();

            string contentHash = Sha256(body ?? "");
            string stringToSign = $"{method}\n{contentHash}\n\n{path}";

            string sign = Sign(_clientId + _accessToken + t + nonce + stringToSign, _secret);

            var req = new HttpRequestMessage(
                method == "GET" ? HttpMethod.Get : HttpMethod.Post,
                path);

            req.Headers.Add("client_id", _clientId);
            req.Headers.Add("access_token", _accessToken);
            req.Headers.Add("t", t);
            req.Headers.Add("nonce", nonce);
            req.Headers.Add("sign", sign);
            req.Headers.Add("sign_method", "HMAC-SHA256");

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    req.Headers.Add(header.Key, header.Value);
                }
            }

            if (method == "POST")
            {
                req.Content = new StringContent(body, Encoding.UTF8, "application/json");
            }

            var resp = await _http.SendAsync(req);
            string json = await resp.Content.ReadAsStringAsync();

            var doc = JsonDocument.Parse(json);

            return doc.RootElement;
        }

        public async Task<JsonElement> GetAsync(string path)
        {
            await EnsureAccessTokenAsync();
            return await SendAsync("GET", path, "", null);
        }

        public Task PostAsync(string path, string bodyJson)
        {
            throw new NotImplementedException();
        }

        public async Task<DeviceListResponse> GetAllDevices()
        {
            await EnsureAccessTokenAsync();
            var jsonElement = await SendAsync("GET", "/v1.0/iot-01/associated-users/devices", null, null);
            try
            {
                TuyaDeviceList tuyaDeviceList = JsonSerializer.Deserialize<TuyaDeviceList>(jsonElement.GetRawText());
                if (tuyaDeviceList == null || (tuyaDeviceList.result.devices.Count == 0))
                {
                    return new DeviceListResponse("Không tìm thấy thiết bị", 404, null);
                }
                else
                {
                    List<DeviceDetail> deviceDetails = new List<DeviceDetail>();
                    foreach (var device in tuyaDeviceList.result.devices)
                    {
                        DeviceDetail deviceDetail = new DeviceDetail
                        {
                            deviceId = device.id,
                            deviceName = device.name,
                            deviceType = device.category,
                            status = device.status
                        };
                        deviceDetails.Add(deviceDetail);
                    }
                    DeviceListResponse deviceListResponse = new DeviceListResponse("Thành công",
                        200,
                        new DeviceListResponse.DeviceListResult
                        {
                            devices = deviceDetails
                        });
                    return deviceListResponse;
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error when GetAllDevices: {ex.Message}");
            }

        }

        public async Task<DeviceDetailResponse> GetDeviceById(string id)
        {
            await EnsureAccessTokenAsync();
            var jsonElement = await SendAsync("GET", $"/v1.0/devices/{id}", null, null);
            try
            {
                TuyaDeviceDetail tuyaDeviceDetail = JsonSerializer.Deserialize<TuyaDeviceDetail>(jsonElement.GetRawText());
                DeviceDetail deviceDetail = new DeviceDetail
                {
                    deviceId = tuyaDeviceDetail.result.id,
                    deviceName = tuyaDeviceDetail.result.name,
                    deviceType = tuyaDeviceDetail.result.category,
                    status = tuyaDeviceDetail.result.status
                };
                DeviceDetailResponse deviceDetailResponse = new DeviceDetailResponse("Thành công", 
                    200, 
                    new DeviceDetailResponse.DeviceDetailResult
                    {
                        device = deviceDetail
                    });
                return deviceDetailResponse;
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error when GetDeviceById {id}: {ex.Message}");
            }
        }

        public async Task<DefaultResponse> SendCommand(string deviceId, List<TuyaDeviceCommand> commands)
        {
            await EnsureAccessTokenAsync();
            // New json structure
            // {"commands": command}
            var jsoncommand = new JsonObject
            {
                ["commands"] = JsonSerializer.Serialize(commands)
            };
            var jsonElement = await SendAsync("POST", $"/v1.0/devices/{deviceId}/commands", jsoncommand.ToString(), null);
            try
            {
                TuyaCommand tuyaCommand = JsonSerializer.Deserialize<TuyaCommand>(jsonElement.GetRawText());
                if (tuyaCommand != null && tuyaCommand.success)
                {
                    return new DefaultResponse("Thành công", 200);
                }
                else
                {
                    return new DefaultResponse("Thất bại khi gửi lệnh đến thiết bị " + deviceId, 400);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error sending command to device {deviceId}: {ex.Message}");
            }
        }
    }
}
