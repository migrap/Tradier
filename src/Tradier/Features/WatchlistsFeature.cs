namespace Tradier.Features {
    public class WatchlistsFeature : IWatchlistsFeature {
        public WatchlistsFeature(TradierClient client) {
            Client = client;
        }

        public TradierClient Client { get; }
    }
}