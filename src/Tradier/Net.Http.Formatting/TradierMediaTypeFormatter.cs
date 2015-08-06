using System;
using System.Collections;
using System.Net.Http.Formatting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Tradier.Net.Http.Formatting {
    internal class TradierMediaTypeFormatter : JsonMediaTypeFormatter {
        public TradierMediaTypeFormatter() {
            SerializerSettings.Converters.Add(new SecuritiesConverter());
        }
    }

    public class SecuritiesConverter : JsonConverter {
        public override bool CanConvert(Type objectType) {
            return typeof(SecurityCollection) == objectType;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            var jobject = JObject.Load(reader);
            var token = jobject.SelectToken("securities").SelectToken("security");

            if(token.Type == JTokenType.Array) {
                return token.ToObject(objectType);
            }

            return new SecurityCollection { token.ToObject<Security>() };
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            throw new NotImplementedException();
        }
    }
}
