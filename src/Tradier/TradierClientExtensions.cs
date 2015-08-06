using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Tradier {
    public static class TradierClientExtensions {
        internal static Task<HttpResponseMessage> GetAsync(this TradierClient client, string requestUri) {
            return client.GetAsync(requestUri, HttpCompletionOption.ResponseContentRead, CancellationToken.None);
        }
    }
}
