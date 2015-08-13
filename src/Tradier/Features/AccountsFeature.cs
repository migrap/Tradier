namespace Tradier.Features {
    public class AccountsFeature : IAccountsFeature {
        public AccountsFeature(TradierClient client) {
            Client = client;
        }

        public TradierClient Client { get; }
    }
}
