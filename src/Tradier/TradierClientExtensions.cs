using System.Collections.Generic;
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

        internal static async Task<HttpResponseMessage> PostAsync(this TradierClient client, string requestUri, IEnumerable<KeyValuePair<string,string>> nameValueCollection) {
            var content = new FormUrlEncodedContent(nameValueCollection);

            return await client.HttpClient.PostAsync(requestUri, content);
        }


        internal static async Task<T> PostAsync<T>(this TradierClient client, string requestUri, IEnumerable<KeyValuePair<string, string>> nameValueCollection) {
            var content = new FormUrlEncodedContent(nameValueCollection);

            var response= await client.HttpClient.PostAsync(requestUri, content);
            return await response.Content.ReadAsAsync<T>(formatters: new[] { Formatter });
        }

        internal static async Task<HttpResponseMessage> DeleteAsync(this TradierClient client, string requestUri) {
            return await client.HttpClient.DeleteAsync(requestUri);
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