using Microsoft.AspNet.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tradier.Features;

namespace Tradier {
    public static class UserFeatureExtensions {
        public static async Task<string> GetProfileAsync(this IUserFeature feature) {
            var path = new PathString("/v1/user/profile");
            var response = await feature.Client.GetAsync(path);

            return await response.Content.ReadAsStringAsync();
        }
    }
}
