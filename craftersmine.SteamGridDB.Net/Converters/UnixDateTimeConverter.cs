using System;
using Newtonsoft.Json;

namespace craftersmine.SteamGridDBNet.Converters
{
    internal class UnixDateTimeConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            long val;
            if (value is DateTime)
            {
                val = ((DateTime)value).ToUnixTime();
            }
            else
            {
                throw new Exception(Resources.Resources.Exception_ExpectedDateTimeObject);
            }
            writer.WriteValue(val);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return new DateTime(1970, 1, 1);

            if (reader.TokenType != JsonToken.Integer)
                throw new Exception(Resources.Resources.Exception_ExpectedIntegerValue);

            long ticks = (long)(reader.Value ?? 0);
            return ticks.FromUnixTime();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(int);
        }
    }
}
