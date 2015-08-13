namespace Tradier.Features {
    public class MarketsFeature : IMarketsFeature {
        public MarketsFeature(TradierClient client) {
            Client = client;
        }

        public TradierClient Client { get; }
    }
}
