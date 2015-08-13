namespace Tradier.Features {
    public class UserFeature : IUserFeature {
        public UserFeature(TradierClient client) {
            Client = client;
        }

        public TradierClient Client { get; }
    }
}