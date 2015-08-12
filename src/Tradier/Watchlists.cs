using Microsoft.AspNet.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Tradier {
    public interface IWatchlistsFeature {
        TradierClient Client { get; }
    }

    public class WatchlistsFeature : IWatchlistsFeature {
        public WatchlistsFeature(TradierClient client) {
            Client = client;
        }

        public TradierClient Client { get; }
    }

    public static partial class WatchlistsFeatureExtensions {
        public static async Task<Watchlists> GetAsync(this IWatchlistsFeature feature) {
            var path = new PathString("/v1/watchlists");

            return await feature.Client.GetAsync<Watchlists>(path);
        }

        public static async Task<HttpResponseMessage> CreateAsync(this IWatchlistsFeature feature, string name, IEnumerable<string> symbols) {
            var path = new PathString("/v1/watchlists");

            var content = new Dictionary<string, string> {
                ["name"] = name,
                ["symbols"] = symbols.Join(",")
            };

            return await feature.Client.PostAsync(path, content);
        }

        public static async Task<HttpResponseMessage> DeleteAsync(this IWatchlistsFeature feature, string id) {
            var path = new PathString("/v1/watchlists")
                .Add("/" + id);

            return await feature.Client.DeleteAsync(path);
        }
    }
}