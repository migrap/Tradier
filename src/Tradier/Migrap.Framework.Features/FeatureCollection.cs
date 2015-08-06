using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace Migrap.Framework.Features {
    internal class FeatureCollection : IFeatureCollection {
        private readonly IFeatureCollection _defaults;
        private readonly ConcurrentDictionary<Type, object> _bytype = new ConcurrentDictionary<Type, object>();
        private readonly ConcurrentDictionary<string, Type> _byname = new ConcurrentDictionary<string, Type>();
        private readonly object _sync = new Object();
        private int _revision;

        public FeatureCollection() {
        }

        public FeatureCollection(IFeatureCollection defaults) {
            _defaults = defaults;
        }

        public object GetInterface() {
            return GetInterface(null);
        }

        public object GetInterface(Type type) {
            var feature = (object)null;
            if (_bytype.TryGetValue(type, out feature)) {
                return feature;
            }

            var actualType = (Type)null;
            if (_byname.TryGetValue(type.FullName, out actualType)) {
                if (_bytype.TryGetValue(actualType, out feature)) {
                    if (type.IsInstanceOfType(feature)) {
                        return feature;
                    }
                    return null;
                }
            }

            if (null != _defaults && _defaults.TryGetValue(type, out feature)) {
                return feature;
            }

            return null;
        }

        private void SetInterface(Type type, object feature) {
            if (type == null) {
                throw new ArgumentNullException("type");
            }
            if (feature == null) {
                throw new ArgumentNullException("feature");
            }

            lock (_sync) {
                var priorFeatureType = (Type)null;
                if (_byname.TryGetValue(type.FullName, out priorFeatureType)) {
                    if (priorFeatureType == type) {
                        _bytype[type] = feature;
                    }
                    else {
                        _byname[type.FullName] = type;
                        _bytype.Remove(priorFeatureType);
                        _bytype.Add(type, feature);
                    }
                }
                else {
                    _byname.Add(type.FullName, type);
                    _bytype.Add(type, feature);
                }
                Interlocked.Increment(ref _revision);
            }
        }

        public virtual int Revision {
            get { return _revision; }
        }

        #region IDisposable Support

        private bool _disposed = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (!_disposed) {
                if (disposing) {
                }
                _disposed = true;
            }
        }

        public void Dispose() {
            Dispose(true);
        }

        #endregion IDisposable Support

        public IEnumerator<KeyValuePair<Type, object>> GetEnumerator() {
            return _bytype.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public void Add(KeyValuePair<Type, object> item) {
            SetInterface(item.Key, item.Value);
        }

        public void Clear() {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<Type, object> item) {
            object value;
            return TryGetValue(item.Key, out value) && Equals(item.Value, value);
        }

        public void CopyTo(KeyValuePair<Type, object>[] array, int arrayIndex) {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<Type, object> item) {
            return Contains(item) && Remove(item.Key);
        }

        public int Count {
            get { throw new NotImplementedException(); }
        }

        public bool IsReadOnly {
            get { return false; }
        }

        public bool ContainsKey(Type key) {
            return GetInterface(key) != null;
        }

        public void Add(Type key, object value) {
            if (ContainsKey(key)) {
                throw new ArgumentException();
            }
            SetInterface(key, value);
        }

        public bool Remove(Type key) {
            var priorFeatureType = (Type)null;
            if (_byname.TryGetValue(key.FullName, out priorFeatureType)) {
                _byname.Remove(key.FullName);
                _bytype.Remove(priorFeatureType);
                Interlocked.Increment(ref _revision);
                return true;
            }
            return false;
        }

        public bool TryGetValue(Type key, out object value) {
            value = GetInterface(key);
            return value != null;
        }

        public object this[Type key] {
            get { return GetInterface(key); }
            set { SetInterface(key, value); }
        }

        public ICollection<Type> Keys {
            get { throw new NotImplementedException(); }
        }

        public ICollection<object> Values {
            get { throw new NotImplementedException(); }
        }
    }
}