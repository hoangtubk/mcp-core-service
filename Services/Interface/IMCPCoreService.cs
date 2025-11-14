using mcp_core_service.Models.Response;

namespace mcp_core_service.Services.Interface
{
    public interface IMCPCoreService
    {
        Task<MusicResponse> SearchMusic(string keyword);
        Task<StreamResponse> StreamMusic(string id);
        Task<OpenWeatherMapResponse> GetWeather(string location, string date);
        Task<AuPriceResponse> GetAuPrice(string trademark, string? type);
        Task<CoinPriceResponse> GetCoinPrice(string coinId, string? currency);
        Task<LotteryResponse> GetLottery(string region, string date);
    }
}
