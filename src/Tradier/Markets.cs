using Microsoft.AspNet.Http;
using System;
using System.Threading.Tasks;

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

    public static partial class MarketsFeatureExtensions {
        public static async Task<SecurityCollection> GetSecuritiesAsync(this IMarketsFeature feature, string value) {
            var path = new PathString("/v1/markets/search").Add(new {
                q = value
            });

            return await feature.Client.GetAsync<SecurityCollection>(path);
        }

        public static async Task<SecurityCollection> GetSecuritiesAsync(this IMarketsFeature feature, string symbol = null, string exchanges = null, string types = null) {
            var path = new PathString("/v1/markets/lookup").Add(new {
                q = symbol,
                exchanges = exchanges,
                types = types
            });
            
            return await feature.Client.GetAsync<SecurityCollection>(path);
        }

        public static async Task<Calendar> GetCalendarAsync(this IMarketsFeature feature, DateTimeOffset? datetime = null) {
            var path = new PathString("/v1/markets/calendar").Add(new {
                month = datetime?.Month,
                year = datetime?.Year
            });
            
            return await feature.Client.GetAsync<Calendar>(path);
        }

        public static async Task<Clock> GetClockAsync(this IMarketsFeature feature) {
            var path = new PathString("/v1/markets/clock");

            return await feature.Client.GetAsync<Clock>(path);            
        }

        public static async Task<History> GetHistoryAsync(this IMarketsFeature feature, string symbol, string interval = "daily", DateTimeOffset? start = null, DateTimeOffset? end = null) {
            var path = new PathString("/v1/markets/history").Add(new {
                symbol = symbol,
                interval = interval,
                start = start?.ToString("yyyy-MM-dd"),
                end = start?.ToString("yyyy-MM-dd")
            });

            return await feature.Client.GetAsync<History>(path);
        }

        public static async Task<Expirations> GetOptionsExpirationsAsync(this IMarketsFeature feature, string symbol) {
            var path = new PathString("/v1/markets/options/expirations").Add(new {
                symbol = symbol
            });

            return await feature.Client.GetAsync<Expirations>(path);
        }

        public static async Task<OptionChain> GetOptionChainAsync(this IMarketsFeature feature, string symbol, string expiration) {
            var path = new PathString("/v1/markets/options/chains").Add(new {
                symbol = symbol,
                expiration = expiration,
            });

            return await feature.Client.GetAsync<OptionChain>(path);
        }

        public static async Task<OptionChain> GetOptionChainAsync(this IMarketsFeature feature, string symbol, DateTimeOffset expiration) {
            return await feature.GetOptionChainAsync(symbol, expiration.ToString("yyyy-MM-dd"));
        }
    }
}