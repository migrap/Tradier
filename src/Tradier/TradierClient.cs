﻿using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Migrap.Framework.Features;

namespace Tradier {
    public class TradierClient {
        private readonly IFeatureCollection _features = new FeatureCollection();
        private FeatureReference<IMarketsFeature> _markets = FeatureReference<IMarketsFeature>.Default;

        internal HttpClient HttpClient { get; } = new HttpClient();

        public TradierClient(string token, bool sandbox = false) {
            HttpClient.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpClient.BaseAddress = new Uri("https://{0}.tradier.com/v1/".FormatWith((sandbox) ? "sandbox" : "api"));
        }

        public IMarketsFeature Markets {
            get { return _markets.Fetch(_features) ?? _markets.Update(_features, new MarketsFeature(this)); }
        }

        internal Task<HttpResponseMessage> GetAsync(string requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken) {
            return HttpClient.GetAsync(requestUri, completionOption, cancellationToken);
        }
    }
}