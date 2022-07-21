using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace craftersmine.SteamGridDBNet.Converters
{
    internal class MimeStringToFormatEnumConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value is SteamGridDbFormats)
            {
                switch ((SteamGridDbFormats)value)
                {
                    case SteamGridDbFormats.Png:
                        writer.WriteValue(SteamGridDbConstants.Mimes.Png);
                        break;
                    case SteamGridDbFormats.Jpeg:
                        writer.WriteValue(SteamGridDbConstants.Mimes.Jpeg);
                        break;
                    case SteamGridDbFormats.Webp:
                        writer.WriteValue(SteamGridDbConstants.Mimes.Webp);
                        break;
                    case SteamGridDbFormats.Ico:
                        writer.WriteValue(SteamGridDbConstants.Mimes.Ico);
                        break;
                }
            }
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                switch (reader.Value)
                {
                    case SteamGridDbConstants.Mimes.Png:
                        return SteamGridDbFormats.Png;
                    case SteamGridDbConstants.Mimes.Jpeg:
                        return SteamGridDbFormats.Jpeg;
                    case SteamGridDbConstants.Mimes.Webp:
                        return SteamGridDbFormats.Webp;
                    case SteamGridDbConstants.Mimes.Ico:
                        return SteamGridDbFormats.Ico;
                }
            }
            return SteamGridDbFormats.Unknown;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }
    }
}
