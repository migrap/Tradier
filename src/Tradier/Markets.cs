using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using Tradier.Net.Http.Formatting;

namespace Tradier {
    public interface IMarketsFeature {
        TradierClient Client { get; }
    }

    public interface IMarketsFeatureExtension {        
    }

    public interface IMarketsFeatureLoookupExtension { }

    public class MarketsFeature : IMarketsFeature {
        public MarketsFeature(TradierClient client) {
            Client = client;
        }

        public TradierClient Client { get; }
    }

    public static class MarketsFeatureExtensions {
        public static async Task<SecurityCollection> SearchAsync(this IMarketsFeature feature, string value) {
            var path = new PathString("/v1/markets/search").Add(new {
                q=value
            });

            var response = await feature.Client.GetAsync(path);

            return await response.Content.ReadAsAsync(x => x.Securities);
        }

        public static async Task<SecurityCollection> LookupAsync(this IMarketsFeature feature, string symbol = null, string exchanges = null, string types = null) {
            var path = new PathString("/v1/markets/lookup").Add(new {
                q = symbol,
                exchanges = exchanges,
                types = types
            });

            var response = await feature.Client.HttpClient.GetAsync(path);
            
            return await response.Content.ReadAsAsync(x => x.Securities);
        }
        
        public static Task<SecurityCollection> Securities(this HttpContent content) {
            var formatters = new[] { new TradierMediaTypeFormatter() };
            return content.ReadAsAsync<SecurityCollection>(formatters, cancellationToken: CancellationToken.None);
        }        
    }
}
