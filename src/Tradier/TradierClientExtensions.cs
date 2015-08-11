using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using Tradier.Net.Http.Formatting;

namespace Tradier {
    public static class TradierClientExtensions {
        private static MediaTypeFormatter Formatter { get; } = new TradierMediaTypeFormatter();

        internal static Task<HttpResponseMessage> GetAsync(this TradierClient client, string requestUri) {
            return client.GetAsync(requestUri, HttpCompletionOption.ResponseContentRead, CancellationToken.None);
        }

        internal static async Task<T> GetAsync<T>(this TradierClient client, string requestUri) {
            return await GetAsync<T>(client, requestUri, Formatter, CancellationToken.None);
        }

        internal static async Task<T> GetAsync<T>(this TradierClient client, string requestUri, MediaTypeFormatter formatter) {
            return await GetAsync<T>(client, requestUri, formatter, CancellationToken.None);
        }

        internal static async Task<T> GetAsync<T>(this TradierClient client, string requestUri, MediaTypeFormatter formatter, CancellationToken cancellationToken) {
            var response = await client.GetAsync(requestUri, HttpCompletionOption.ResponseContentRead, cancellationToken);

            if(typeof(T) == typeof(string)) {
                var result = await response.Content.ReadAsStringAsync();
                return (T)((object)result);
            }

            return await response.Content.ReadAsAsync<T>(formatters: new[] { formatter }, cancellationToken: cancellationToken);
        }
    }
}