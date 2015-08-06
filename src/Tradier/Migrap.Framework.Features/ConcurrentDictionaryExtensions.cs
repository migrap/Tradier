using System.Collections.Concurrent;

namespace Migrap.Framework.Features {
    internal static class ConcurrentDictionaryExtensions {
        public static void Add<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary, TKey key, TValue value) {
            dictionary.TryAdd(key, value);
        }

        public static void Remove<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary, TKey key) {
            var value = default(TValue);
            dictionary.TryRemove(key, out value);
        }
    }
}
