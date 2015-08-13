using Microsoft.AspNet.Http;
using System.Threading.Tasks;
using Tradier.Features;

namespace Tradier {
    public static class AccountsFeatureExtensions {
        public static async Task<string> GetAsync(this IAccountsFeature feature) {
            //var path = new PathString("/v1/user/profile");
            var path = new PathString("/v1/accounts");
            var result = await feature.Client.GetAsync(path);

            return await result.Content.ReadAsStringAsync();
        }
    }
}
