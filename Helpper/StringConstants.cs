namespace mcp_core_service.Helpper
{
    public class StringConstants
    {
        public static string URL_OPEN_STREET_MAP = "https://nominatim.openstreetmap.org/";
        public static string URL_OPEN_WEATHER_MAP = "https://api.openweathermap.org/data/3.0/onecall/";
        public static string URL_AU_BTMC = "http://api.btmc.vn/api/BTMCAPI/getpricebtmc?key=3kd8ub1llcg9t45hnoh8hmn7t5kc2v";
        public static string URL_COIN_BASE = "https://api.coinbase.com/v2/";
        public static string URL_XSKT_XSMB = "https://xskt.com.vn/rss-feed/";
        
        public static string API_KEY = "your_api_key"; //https://home.openweathermap.org/api_keys

        public class AU_UNIT
        {
            public static string VND = "VND/1 chỉ";
            public static string USD = "USD/ 1 ounce";
        }

        public class CURRENCY
        {
            public static string VND = "VND";
            public static string USD = "USD";
        }

        public class TRADEMARK
        {
            public static string BTMC = "BTMC";
            public static string GLOBAL = "GLOBAL";
        }

        public class LOTTERY_REGION
        {
            public static string MIEN_BAC = "XSMB";
            public static string MIEN_TRUNG = "XSMT";
            public static string MIEN_NAM = "XSMN";
        }
    }
}
