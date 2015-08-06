using System.Threading;

namespace Migrap.Framework.Features {
    internal struct FeatureReference<T> {
        private T _feature;
        private int _revision;

        private FeatureReference(T feature, int revision) {
            _feature = feature;
            _revision = revision;
        }

        public static readonly FeatureReference<T> Default = new FeatureReference<T>(default(T), -1);

        public T Fetch(IFeatureCollection features) {
            if (_revision == features.Revision) {
                return _feature;
            }
            var value = (object)null;
            if (features.TryGetValue(typeof(T), out value)) {
                _feature = (T)value;
            }
            else {
                _feature = default(T);
            }
            Interlocked.Exchange(ref _revision, features.Revision);
            return _feature;
        }

        public T Update(IFeatureCollection features, T feature) {
            features[typeof(T)] = _feature = feature;
            Interlocked.Exchange(ref _revision, features.Revision);
            return feature;
        }
    }
}
