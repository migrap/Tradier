using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using System.Linq;

namespace Tradier {
    public static partial class Extensions {
        internal static string FormatWith(this string format, params string[] args) {
            return string.Format(format, args);
        }

        internal static string Join(this IEnumerable<string> values, string seperator) {
            return string.Join(seperator, values);
        }

        internal static bool IsNotNullOrWhiteSpace(this string source) {
            return !string.IsNullOrWhiteSpace(source);
        }

        internal static bool IsNotNullOrEmpty(this string source) {
            return !string.IsNullOrEmpty(source);
        }       
        

        internal static string Add(this PathString path, object value) {
            var parameters = value.GetType().GetProperties().ToDictionary(x => x.Name, x => x.GetValue(value)?.ToString())
                .Where(x => x.Value.IsNotNullOrWhiteSpace());

            var query = new QueryString();

            foreach(var parameter in parameters) {
                query = query.Add(parameter.Key, parameter.Value);
            }

            return path.Add(query);
        }
    }    
}
