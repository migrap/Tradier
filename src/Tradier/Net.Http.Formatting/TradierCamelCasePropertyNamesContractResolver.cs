using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net.Http.Formatting;
using System.Reflection;
using Tradier.Net.Http.Formatting.Converters;

namespace Tradier.Net.Http.Formatting {
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

            if(property.DeclaringType == typeof(Watchlist) && property.PropertyName.Equals("items", StringComparison.OrdinalIgnoreCase)) {
                property.Converter = property.MemberConverter = new CollectionConverter<Watchlist.Item>("item");
            }

            return property;
        }
    }
}
