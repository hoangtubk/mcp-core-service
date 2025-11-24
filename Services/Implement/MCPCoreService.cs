using mcp_core_service.Middlewares;
using mcp_core_service.Models.Response;
using mcp_core_service.Services.Interface;
using Newtonsoft.Json;
using RandomUserAgent;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using ZingMP3Explode;
using ZingMP3Explode.Entities;
using static mcp_core_service.Helpper.CustomJsonConverter;
using static mcp_core_service.Helpper.StringConstants;
using static mcp_core_service.Models.Response.LotteryResponse;
using static mcp_core_service.Models.Response.StreamResponse;

namespace mcp_core_service.Services.Implement
{
    public class MCPCoreService : IMCPCoreService
    {
        private static string SanitizeResponse(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            // Remove all '@' characters
            string noAt = input.Replace("@", "");
            // Remove '_' and the following word characters (letters, digits, underscore)
            string sanitized = Regex.Replace(noAt, "_\\w+", "");
            return sanitized;
        }

        public async Task<AuPriceResponse> GetAuPrice(string trademark, string? type)
        {
            List<AuPrice> auPrices = new List<AuPrice>();

            // Gọi API lấy giá vàng BTMC
            if (trademark.ToUpper() == TRADEMARK.BTMC)
            {
                string urlBTMC = $"{URL_AU_BTMC}";
                using var httpClient = new HttpClient();
                try
                {
                    HttpResponseMessage result = await httpClient.GetAsync(urlBTMC);
                    string response = await result.Content.ReadAsStringAsync();
                    if (result.IsSuccessStatusCode && !string.IsNullOrEmpty(response))
                    {
                        response = SanitizeResponse(response);

                        var settings = new JsonSerializerSettings
                        {
                            Converters =
                            {
                                new CustomDateTimeConverter(),
                                new CustomNullableDateTimeConverter()
                            }
                        };
                        BTMCResponse bTMCResponse = JsonConvert.DeserializeObject<BTMCResponse>(response, settings);

                        if (bTMCResponse != null && bTMCResponse.DataList != null && bTMCResponse.DataList.data != null)
                        {
                            List<ProfilePrice> profiles = new List<ProfilePrice>();
                            if (string.IsNullOrEmpty(type)) type = "";

                            // Normalize type for comparison
                            string typeNormalized = type!.ToLower();

                            // Take unique items by 'n', keeping the one with the most recent 'd'
                            var uniqueData = bTMCResponse.DataList.data
                                .Where(x => !string.IsNullOrEmpty(x.n))
                                .GroupBy(x => x.n)
                                .Select(g => g.OrderByDescending(x => x.d ?? DateTime.MinValue).First())
                                .ToList();

                            foreach (var item in uniqueData)
                            {
                                if (item.n != null && item.n.ToLower().Contains(typeNormalized))
                                {
                                    ProfilePrice profile = new ProfilePrice
                                    {
                                        type = item.k,
                                        desc = item.n,
                                        bid = item.pb,
                                        ask = item.ps,
                                        unit = AU_UNIT.VND,
                                        updated_at = item.d
                                    };
                                    profiles.Add(profile);
                                }
                            }
                            AuPrice auPrice = new AuPrice
                            {
                                trademark = TRADEMARK.BTMC,
                                profile = profiles
                            };
                            auPrices.Add(auPrice);
                        }
                        else
                        {
                            throw new NotFoundException("trademark/type", $"{trademark}/{type}");
                        }
                    }
                    else
                    {
                        throw new NotFoundException("trademark/type", $"{trademark}/{type}");
                    }
                }
                catch (Exception ex)
                {
                    throw new BusinessException($"Error searching Au Price: {ex.Message}");
                }
            }
            // Gọi API lấy giá vàng SJC
            // Gọi API lấy giá vàng DOJI
            // Gọi API lấy giá vàng PNJ
            // Gọi API lấy giá vàng thế giới
            else if (trademark.ToUpper() == TRADEMARK.GLOBAL)
            {
                string urlCoinBase = $"{URL_COIN_BASE}prices/XAU-USD/spot";
                using var httpClient = new HttpClient();
                try
                {
                    HttpResponseMessage result = await httpClient.GetAsync(urlCoinBase);
                    string response = await result.Content.ReadAsStringAsync();
                    if (result.IsSuccessStatusCode && !string.IsNullOrEmpty(response))
                    {
                        CoinBaseResponse coinBaseResponse = JsonConvert.DeserializeObject<CoinBaseResponse>(response);
                        if(coinBaseResponse == null || coinBaseResponse.data == null || string.IsNullOrEmpty(coinBaseResponse.data.amount))
                        {
                            throw new NotFoundException("trademark", trademark);
                        }
                        List<ProfilePrice> profiles = new List<ProfilePrice>();
                        ProfilePrice profile = new ProfilePrice
                        {
                            type = coinBaseResponse.data.@base,
                            desc = coinBaseResponse.data.@base,
                            bid = coinBaseResponse.data.amount,
                            ask = coinBaseResponse.data.amount,
                            unit = AU_UNIT.USD,
                            updated_at = DateTime.UtcNow.AddHours(7),
                        };
                        profiles.Add(profile);
                        AuPrice auPrice = new AuPrice
                        {
                            trademark = TRADEMARK.GLOBAL,
                            profile = profiles
                        };
                        auPrices.Add(auPrice);
                    }
                }
                catch (Exception ex)
                {
                    throw new BusinessException($"Error searching Au Price: {ex.Message}");
                }
            }
            else
            {
                throw new NotFoundException("trademark", trademark);
            }

            AuPriceResponse auPriceResponses = new AuPriceResponse("Lấy giá vàng thành công", 200, auPrices);
            return auPriceResponses;
        }

        public async Task<CoinPriceResponse> GetCoinPrice(string coinId, string? currency)
        {
            if (string.IsNullOrEmpty(currency))
            {
                currency = CURRENCY.USD;
            }
            coinId = coinId.ToUpper();
            currency = currency.ToUpper();

            string urlCoinBase = $"{URL_COIN_BASE}prices/{coinId}-{currency}/spot";
            using var httpClient = new HttpClient();
            try
            {
                HttpResponseMessage result = await httpClient.GetAsync(urlCoinBase);
                string response = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode && !string.IsNullOrEmpty(response))
                {
                    CoinBaseResponse coinBaseResponse = JsonConvert.DeserializeObject<CoinBaseResponse>(response);
                    if (coinBaseResponse == null || coinBaseResponse.data == null || string.IsNullOrEmpty(coinBaseResponse.data.amount))
                    {
                        throw new NotFoundException("coinId", coinId);
                    }
                    return new CoinPriceResponse("Lấy giá coin thành công", 200, new CoinPriceResponse.CoinPriceResult
                    {
                        CoinId = coinId,
                        Currency = currency,
                        Amount = coinBaseResponse.data.amount,
                    });
                }
                else
                {
                    throw new NotFoundException("coinId", coinId);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error searching Coin Price: {ex.Message}");
            }
        }

        public async Task<LotteryResponse> GetLottery(string region, string date)
        {
            // 1. Validate date presence
            if (string.IsNullOrWhiteSpace(date))
                throw new BusinessException("Ngày không được để trống.");

            // 2. Parse date in exact format yyyy-MM-dd
            if (!DateTime.TryParseExact(
                    date,
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime parsedDate))
            {
                throw new BusinessException("Định dạng ngày không hợp lệ. Vui lòng sử dụng yyyy-MM-dd.");
            }

            // 3. Check not future relative to Vietnam local date (UTC+7)
            DateTime vietnamToday = DateTime.UtcNow.AddHours(7).Date;
            if (parsedDate.Date > vietnamToday)
                throw new BusinessException("Ngày không được lớn hơn ngày hiện tại.");

            // 4. Validate region
            if (string.IsNullOrWhiteSpace(region))
                throw new BusinessException("Vùng (region) không được để trống.");

            // 5. Handle supported region(s) (placeholder for actual implementation)
            // TODO: Implement actual API call to retrieve lottery results for 'Miền Bắc' on the given date.
            string urlXSKT = $"{URL_XSKT_XSMB}{region}.rss";
            using var httpClient = new HttpClient();
            try
            {
                HttpResponseMessage result = await httpClient.GetAsync(urlXSKT);
                string xmlresponse = await result.Content.ReadAsStringAsync();
                                        
                if (result.IsSuccessStatusCode && !string.IsNullOrEmpty(xmlresponse))
                {
                    var serializer = new XmlSerializer(typeof(XSKTResponse));
                    using (var reader = new StringReader(xmlresponse))
                    {
                        XSKTResponse xsktResponse = (XSKTResponse)serializer.Deserialize(reader);
                        if (xsktResponse == null 
                            || xsktResponse.Channel == null 
                            || xsktResponse.Channel.Items == null 
                            || xsktResponse.Channel.Items.Count == 0)
                        {
                            throw new NotFoundException("region/date", $"{region}/{date}");
                        }
                        foreach (var item in xsktResponse.Channel.Items)
                        {
                            // Regex lấy phần ngày dạng dd-MM-yyyy
                            var matchDate = Regex.Match(item.Link?? "", @"(\d{1,2}-\d{1,2}-\d{4})");

                            DateTime.TryParseExact(
                            matchDate.Value,
                            "d-M-yyyy",
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.AdjustToUniversal,
                            out DateTime parsedDateItem);

                            if (parsedDateItem.Date != parsedDate.Date) continue;

                            // Nếu đúng ngày thì xử lý kết quả xổ số ở đây
                            List<LotteryDetail> lotteryDetails = new List<LotteryDetail>();
                            // Regex: tách từng phần kiểu "key: numbers"
                            var matches = Regex.Matches(item.Description ?? "", @"(\S+):\s*([^\s]+(?:\s*-\s*[^\s]+)*)");

                            foreach (Match match in matches)
                            {
                                string key = match.Groups[1].Value;
                                string numbersPart = match.Groups[2].Value;

                                // Tách số bằng dấu '-'
                                var numbers = new List<string>();
                                foreach (var n in numbersPart.Split('-', StringSplitOptions.RemoveEmptyEntries))
                                {
                                    numbers.Add(n.Trim());
                                }

                                lotteryDetails.Add(new LotteryDetail
                                {
                                    key = key,
                                    numbers = numbers
                                });
                            }

                            LotteryResult lotteryResult = new LotteryResult
                            {
                                region = region,
                                lotteryTitle = xsktResponse.Channel.Title ?? "",
                                lotteryDesc = xsktResponse.Channel.Description ?? "",
                                lotteryDate = parsedDate.Date.ToString("yyyy-MM-dd"),
                                lotteryDetails = lotteryDetails,
                            };
                            return new LotteryResponse("Lấy kết quả xổ số thành công", 200, lotteryResult);
                        }
                        throw new NotFoundException("date", parsedDate);
                    }
                }
                else
                {
                    throw new NotFoundException("region/date", $"{region}/{date}");
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error searching Lottery: {ex.Message}");
            }

            //// 6. Unsupported region
            //throw new NotFoundException("region", region);
        }

        public async Task<OpenWeatherMapResponse> GetWeather(string location, string date)
        {
            // Bước 1: Cần lấy được tọa độ (latitude, longitude) từ location
            float latitude;
            float longitude;
            string urlOpenStreetMap = $"{URL_OPEN_STREET_MAP}search?q={location}&format=json&limit=1";
            using var httpClientStreet = new HttpClient();
            httpClientStreet.DefaultRequestHeaders.Add("user-agent", RandomUa.RandomUserAgent);
            try
            {
                HttpResponseMessage result = await httpClientStreet.GetAsync(urlOpenStreetMap);
                string response = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode && !string.IsNullOrEmpty(response))
                {
                    List<OpenStreetMap> openStreetMap = JsonConvert.DeserializeObject<List<OpenStreetMap>>(response);
                    if (openStreetMap != null && openStreetMap.Count > 0)
                    {
                        var locationData = openStreetMap.First();
                        latitude = locationData.lat;
                        longitude = locationData.lon;
                    }
                    else
                    {
                        throw new NotFoundException("location", location);
                    }
                }
                else
                {
                    throw new NotFoundException("location", location);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error searching lat/lon location: {ex.Message}");
            }
            // Bước 2: Sử dụng openweathermap - day_summary để lấy dữ liệu thời tiết
            // Gọi GET đến Daily Aggregation truyền vào tọa độ và ngày cần lấy dữ liệu
            string apiKey = Environment.GetEnvironmentVariable("API_KEY_WEATHER_MAP") ?? API_KEY_WEATHER_MAP;
            string urlOpenWeatherMap = $"{URL_OPEN_WEATHER_MAP}day_summary?lat={latitude}&lon={longitude}&date={date}&units=metric&appid={apiKey}";
            using var httpClientWeather = new HttpClient();
            try
            {
                HttpResponseMessage result = await httpClientWeather.GetAsync(urlOpenWeatherMap);
                string response = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode && !string.IsNullOrEmpty(response))
                {
                    OpenWeatherMap weatherData = JsonConvert.DeserializeObject<OpenWeatherMap>(response);
                    if (weatherData != null)
                    {
                        return new OpenWeatherMapResponse("Lấy thông tin thời tiết thành công", 200, weatherData);
                    }
                    else
                    {
                        throw new NotFoundException("location/date", $"{location}/{date}");
                    }
                }
                else
                {
                    throw new NotFoundException("location/date", $"{location}/{date}");
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error GET weather day summary: {ex.Message}");
            }
        }

        public async Task<MusicResponse> SearchMusic(string keyword)
        {
            try
            {
                var client = new ZingMP3Client();
                await client.InitializeAsync();
                MultiSearchResult searchResults = await client.Search.SearchMultiAsync(keyword);

                var results = searchResults.Songs.Select(song => new MusicResult
                {
                    Id = song.ID,
                    Title = song.Title,
                    Artist = song.AllArtistsNames,
                    FullUrl = song.FullUrl,
                    Duration = song.Duration,
                }).ToList();

                return new MusicResponse("Thành công", 200, results);
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error searching music: {ex.Message}");
            }
        }

        public async Task<StreamResponse> StreamMusic(string id)
        {
            try
            {
                var client = new ZingMP3Client();
                await client.InitializeAsync();

                string audioStreamUrl = await client.Songs.GetAudioStreamUrlAsync(id, AudioQuality.Best);
                Song song = await client.Songs.GetAsync(id);
                string title = song.Title;
                string artist = song.AllArtistsNames;
                long duration = song.Duration;
                string fullUrl = song.FullUrl;

                StreamResult audioStreamResult = new StreamResult
                {
                    Id = id,
                    StreamUrl = audioStreamUrl,
                    Title = title,
                    Artist = artist,
                    FullUrl = fullUrl,
                    Duration = duration,
                };

                return new StreamResponse("Thành công", 200, audioStreamResult);
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error streaming music: {ex.Message}");
            }
        }

        public async Task<LyricResponse> GetLyrics(string id)
        {
            try
            {
                var client = new ZingMP3Client();
                await client.InitializeAsync();

                string audioStreamUrl = await client.Songs.GetAudioStreamUrlAsync(id, AudioQuality.Best);
                Song song = await client.Songs.GetAsync(id);
                LyricData lyrics = await client.Songs.GetLyricsAsync(song.FullUrl);

                return new LyricResponse("Thành công", 200, lyrics);
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error get lyrics: {ex.Message}");
            }
        }
    }
}
