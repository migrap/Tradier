using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Net.Http.Formatting;
using System.Reflection;

namespace Tradier.Net.Http.Formatting {
    internal class TradierMediaTypeFormatter : JsonMediaTypeFormatter {
        public TradierMediaTypeFormatter() {
            SerializerSettings.ContractResolver = new TradierCamelCasePropertyNamesContractResolver(this);

            SerializerSettings.Converters.Add(new SecuritiesConverter());
            SerializerSettings.Converters.Add(new CalendarConverter());
            SerializerSettings.Converters.Add(new ClockConverter());
            SerializerSettings.Converters.Add(new HistoryConverter());
            SerializerSettings.Converters.Add(new ExpirationsConverter());
            SerializerSettings.Converters.Add(new OptionChainConverter());
            SerializerSettings.Converters.Add(new WatchlistsConverter());
        }
    }

    internal class TradierCamelCasePropertyNamesContractResolver : JsonContractResolver {
        public TradierCamelCasePropertyNamesContractResolver(MediaTypeFormatter formatter)
            : base(formatter) {

        }

        protected override string ResolvePropertyName(string propertyName) {
            return GetSnakeCase(propertyName);
        }

        private string GetSnakeCase(string input) {
            if(string.IsNullOrEmpty(input)) {
                return input;
            }

            var buffer = "";

            for(var i = 0; i < input.Length; i++) {
                var isLast = (i == input.Length - 1);
                var isSecondFromLast = (i == input.Length - 2);

                var curr = input[i];
                var next = !isLast ? input[i + 1] : '\0';
                var afterNext = !isSecondFromLast && !isLast ? input[i + 2] : '\0';

                buffer += char.ToLower(curr);

                if(!char.IsDigit(curr) && char.IsUpper(next)) {
                    if(char.IsUpper(curr)) {
                        if(!isLast && !isSecondFromLast && !char.IsUpper(afterNext)) {
                            buffer += "_";
                        }
                    }
                    else {
                        buffer += "_";
                    }
                }

                if(!char.IsDigit(curr) && char.IsDigit(next)) {
                    buffer += "_";
                }
                if(char.IsDigit(curr) && !char.IsDigit(next) && !isLast) {
                    buffer += "_";
                }
            }

            return buffer;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization) {
            var property = base.CreateProperty(member, memberSerialization);

            if(property.DeclaringType == typeof(Clock) && property.PropertyName.Equals("timestamp", StringComparison.OrdinalIgnoreCase)) {
                property.Converter = property.MemberConverter = new EpochDateTimeOffsetConverter();
            }

            return property;
        }
    }

    internal class EpochDateTimeOffsetConverter : JsonConverter {
        private static DateTimeOffset Epoch = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);
        public EpochDateTimeOffsetConverter() {
        }

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

    public class SecuritiesConverter : Converter<SecurityCollection> {
        public SecuritiesConverter() : base("securities.security") {
        }

        protected override object Convert(JToken token, JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            if(token.Type == JTokenType.Array) {
                return token.ToObject(objectType);
            }

            return new SecurityCollection { token.ToObject<Security>() };
        }
    }

    public class HistoryConverter : Converter<History> {
        public HistoryConverter() : base("history.day") {
        }

        protected override object Convert(JToken token, JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            if(token.Type == JTokenType.Array) {
                return token.ToObject(objectType);
            }

            return new History { token.ToObject<History.Day>() };
        }
    }

    public class ExpirationsConverter : Converter<Expirations> {
        public ExpirationsConverter() : base("expirations.date") {
        }

        protected override object Convert(JToken token, JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            if(token.Type == JTokenType.Array) {
                return token.ToObject(objectType);
            }

            return new Expirations { token.ToObject<DateTimeOffset>() };
        }
    }

    public class OptionChainConverter : Converter<OptionChain> {
        public OptionChainConverter() : base("options.option") {
        }

        protected override object Convert(JToken token, JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            if(token.Type == JTokenType.Array) {
                return token.ToObject(objectType);
            }

            return new OptionChain { token.ToObject<Option>() };
        }
    }

    public class WatchlistsConverter : Converter<Watchlists> {
        public WatchlistsConverter() : base("watchlists.watchlist") {
        }

        protected override object Convert(JToken token, JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            var target = (object)null;
            if(token.Type == JTokenType.Array) {
                target = new Watchlists();
            }
            else {
                target = new Watchlist();
            }

            serializer.Populate(token.CreateReader(), target);

            if(target is Watchlist) {
                target = new Watchlists { (Watchlist)target };
            }

            return target;
        }
    }

    public class CalendarConverter : Converter<Calendar> {
        public CalendarConverter() : base("calendar.days.day") {
        }
    }

    public class ClockConverter : Converter<Clock> {
        public ClockConverter() : base("clock") {
        }
    }

    public class Converter<T> : JsonConverter where T : class, new() {
        private readonly string _path;

        public Converter(string path) {
            _path = path;
        }

        public override bool CanConvert(Type objectType) {
            return typeof(T).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            var token = JObject.Load(reader)
                .SelectToken(_path);

            return Convert(token, reader, objectType, existingValue, serializer);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            throw new NotImplementedException();
        }

        protected virtual object Convert(JToken token, JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            var target = new T();

            serializer.Populate(token.CreateReader(), target);

            return target;
        }
    }
}