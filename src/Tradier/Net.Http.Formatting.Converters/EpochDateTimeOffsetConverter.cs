using Newtonsoft.Json;
using System;

namespace Tradier.Net.Http.Formatting.Converters {
    internal class EpochDateTimeOffsetConverter : JsonConverter {
        private static readonly DateTimeOffset Epoch = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);

        public override bool CanConvert(Type objectType) {
            return typeof(DateTimeOffset).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            if(reader.TokenType == JsonToken.Integer) {
                var value = (long)reader.Value;
                return Epoch.AddSeconds(value);
            }

            if(reader.TokenType == JsonToken.String) {
                var value = long.Parse((string)reader.Value);
                return Epoch.AddMilliseconds(value / 1000);
            }

            throw new JsonReaderException(string.Format("Unexcepted token {0}", reader.TokenType));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            throw new NotImplementedException();
        }
    }
}
