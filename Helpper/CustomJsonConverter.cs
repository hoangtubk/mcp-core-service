using Newtonsoft.Json;
using System.Globalization;

namespace mcp_core_service.Helpper
{
    public class CustomJsonConverter
    {
        public class CustomDateTimeConverter : JsonConverter<DateTime>
        {
            private readonly string[] formats = {
                "dd/MM/yyyy HH:mm",
                "dd/MM/yyyy",
                "dd-MM-yyyy HH:mm",
                "dd-MM-yyyy"
            };

            public override DateTime ReadJson(JsonReader reader, Type objectType,
                DateTime existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                var s = reader.Value?.ToString();
                if (DateTime.TryParseExact(s, formats,
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out var dt))
                {
                    return dt;
                }

                throw new JsonReaderException($"Invalid date format: {s}");
            }

            public override void WriteJson(JsonWriter writer, DateTime value, JsonSerializer serializer)
            {
                writer.WriteValue(value.ToString("dd/MM/yyyy HH:mm"));
            }
        }
        public class CustomNullableDateTimeConverter : JsonConverter<DateTime?>
        {
            private readonly string[] formats = {
                "dd/MM/yyyy HH:mm",
                "dd/MM/yyyy",
                "dd-MM-yyyy HH:mm",
                "dd-MM-yyyy"
            };

            public override DateTime? ReadJson(JsonReader reader, Type objectType,
                DateTime? existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                var s = reader.Value?.ToString();

                if (string.IsNullOrEmpty(s))
                    return null;

                if (DateTime.TryParseExact(
                        s,
                        formats,
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out var dt))
                {
                    return dt;
                }

                throw new JsonReaderException($"Invalid date format: {s}");
            }

            public override void WriteJson(JsonWriter writer, DateTime? value, JsonSerializer serializer)
            {
                writer.WriteValue(value?.ToString("dd/MM/yyyy HH:mm"));
            }
        }
    }
}
