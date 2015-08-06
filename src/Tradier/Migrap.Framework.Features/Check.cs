using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

internal static partial class CheckExtensions {
    internal static string CheckNotNull(this string value) {
        if (null == value) {
            throw new ArgumentNullException("value");
        }
        return value;
    }

    internal static T CheckNotNull<T>(this T value) where T : class {
        if (null == value) {
            throw new ArgumentNullException("value");
        }
        return NullChecker<T>.Check(value);
    }

    internal static void Check<T>(this T value, Func<T, bool> callback, Func<Exception> exception) where T : class {
        if (null == value) {
            throw new ArgumentNullException("value");
        }
        FuncChecker<T>.Check(value, callback, exception);
    }

    private static class NullChecker<T> where T : class {
        private static readonly List<Func<T, bool>> _checkers = new List<Func<T, bool>>();
        private static readonly List<string> _names = new List<string>();

        static NullChecker() {
            foreach (var name in typeof(T).GetConstructors()[0].GetParameters().Select(p => p.Name)) {
                _names.Add(name);
                var property = typeof(T).GetProperty(name);

                if (property.PropertyType.IsValueType) {
                    throw new ArgumentException("Property " + property + " is a value type");
                }

                var parameter = Expression.Parameter(typeof(T), "value");
                var accessor = Expression.Property(parameter, property);
                var nullvalue = Expression.Constant(null, property.PropertyType);
                var equality = Expression.Equal(accessor, nullvalue);
                var lambda = Expression.Lambda<Func<T, bool>>(equality, parameter);

                _checkers.Add(lambda.Compile());
            }
        }

        internal static T Check(T value) {
            for (int i = 0; i < _checkers.Count; i++) {
                if (_checkers[i](value)) {
                    throw new ArgumentNullException(_names[i]);
                }
            }
            return value;
        }
    }

    private static class FuncChecker<T> where T : class {
        internal static void Check(T value, Func<T, bool> callback, Func<Exception> exception) {
            if (false == callback(value)) {
                throw exception();
            }
        }
    }
}
