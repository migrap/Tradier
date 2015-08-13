using Microsoft.AspNet.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tradier.Features;

namespace Tradier {
    public static partial class WatchlistsFeatureExtensions {
        public static async Task<WatchlistCollection> GetAsync(this IWatchlistsFeature feature) {
            var path = new PathString("/v1/watchlists");

            return await feature.Client.GetAsync<WatchlistCollection>(path);
        }

        public static async Task<Watchlist> GetAsync(this IWatchlistsFeature feature, string id) {
            var path = new PathString("/v1/watchlists")
                .Add("/" + id);

            return await feature.Client.GetAsync<Watchlist>(path);
        }

        public static async Task<Watchlist> CreateAsync(this IWatchlistsFeature feature, string name) {
            var path = new PathString("/v1/watchlists");

            var content = new Dictionary<string, string> {
                ["name"] = name
            };

            return await feature.Client.PostAsync<Watchlist>(path, content);
        }

        public static async Task<Watchlist> CreateAsync(this IWatchlistsFeature feature, string name, IEnumerable<string> symbols) {
            var path = new PathString("/v1/watchlists");

            var content = new Dictionary<string, string> {
                ["name"] = name,
                ["symbols"] = symbols.Join(",")
            };

            return await feature.Client.PostAsync<Watchlist>(path, content);
        }

        public static async Task<WatchlistCollection> DeleteAsync(this IWatchlistsFeature feature, string id) {
            var path = new PathString("/v1/watchlists")
                .Add("/" + id);

            return await feature.Client.DeleteAsync<WatchlistCollection>(path);
        }

        public static async Task<Watchlist> UpdateAsync(this IWatchlistsFeature feature, string id, IEnumerable<string> symbols) {
            var path = new PathString("/v1/watchlists")
                .Add("/" + id)
                .Add("/symbols");

            var content = new Dictionary<string, string> {
                ["symbols"] = symbols.Join(",")
            };

            return await feature.Client.PostAsync<Watchlist>(path, content);
        }
    }
}