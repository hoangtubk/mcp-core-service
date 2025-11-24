using mcp_core_service.Models.Response;
using mcp_core_service.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace mcp_core_service.Controllers
{
    [Route("mcpcore/v1")]
    [ApiController]
    public class MCPController : ControllerBase
    {
        private readonly IMCPCoreService _mcpCoreService;
        public MCPController(IMCPCoreService mCPCoreService)
        {
            _mcpCoreService = mCPCoreService;
        }

        [HttpGet("search_music")]
        public async Task<MusicResponse> SearchMusic([FromQuery] string keyword)
        {
            MusicResponse response = await _mcpCoreService.SearchMusic(keyword);
            return response;
        }

        [HttpGet("stream_music")]
        public async Task<StreamResponse> StreamMusic([FromQuery] string id)
        {
            StreamResponse response = await _mcpCoreService.StreamMusic(id);
            return response;
        }

        [HttpGet("get_lyrics")]
        public async Task<LyricResponse> GetLyrics([FromQuery] string id)
        {
            LyricResponse response = await _mcpCoreService.GetLyrics(id);
            return response;
        }

        [HttpGet("get_weather")]
        public async Task<OpenWeatherMapResponse> GetWeather([FromQuery] string location, [FromQuery] string date)
        {
            OpenWeatherMapResponse response = await _mcpCoreService.GetWeather(location, date);
            return response;
        }

        [HttpGet("get_au_price")]
        public async Task<AuPriceResponse> GetAuPrice([FromQuery] string trademark, [FromQuery] string? type)
        {
            // http://api.btmc.vn/api/BTMCAPI/getpricebtmc?key=3kd8ub1llcg9t45hnoh8hmn7t5kc2v
            AuPriceResponse response = await _mcpCoreService.GetAuPrice(trademark, type);
            return response;
        }

        [HttpGet("get_lottery")]
        public async Task<LotteryResponse> GetLottery([FromQuery] string region, [FromQuery] string date)
        {
            // https://xskt.com.vn/rss
            LotteryResponse response = await _mcpCoreService.GetLottery(region, date);
            return response;
        }

        [HttpGet("get_coin_price")]
        public async Task<CoinPriceResponse> GetCoinPrice([FromQuery] string coinId, [FromQuery] string? currency)
        {
            // https://api.coinbase.com/v2/prices/BTC-USD/spot
            CoinPriceResponse response = await _mcpCoreService.GetCoinPrice(coinId, currency);
            return response;
        }
    }
}
