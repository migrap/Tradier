using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
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
            SerializerSettings.Converters.Add(new WatchlistCollectionConverter());
            SerializerSettings.Converters.Add(new WatchlistConverter());
        }
    }

    

    internal class CollectionConverter<T> : Converter<T> where T : class, new() {
        public CollectionConverter(string path) : base(path) {
        }

        protected override object Convert(JToken token, JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            var target = (object)null;

            if(token.Type == JTokenType.Array) {
                target = new List<T>();
            }
            else {
                target = new T();
            }

            serializer.Populate(token.CreateReader(), target);

            if(target is T) {
                target = new List<T> { (T)target };
            }

            return target;
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

    public class WatchlistCollectionConverter : Converter<WatchlistCollection> {
        public WatchlistCollectionConverter() : base("watchlists.watchlist") {
        }

        protected override object Convert(JToken token, JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            var target = (object)null;
            if(token.Type == JTokenType.Array) {
                target = new WatchlistCollection();
            }
            else {
                target = new Watchlist();
            }

            serializer.Populate(token.CreateReader(), target);

            if(target is Watchlist) {
                target = new WatchlistCollection { (Watchlist)target };
            }

            return target;
        }
    }

    public class WatchlistConverter : Converter<Watchlist> {
        public WatchlistConverter() : base("watchlist") {
        }

        protected override object Convert(JToken token, JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            var target = new Watchlist();

            serializer.Populate(token?.CreateReader() ?? reader, target);

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
            var jobject = JObject.Load(reader);
            var token = jobject.SelectToken(_path) ?? jobject;

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