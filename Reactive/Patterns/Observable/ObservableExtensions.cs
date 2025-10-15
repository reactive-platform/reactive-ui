using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Reactive {
    [PublicAPI]
    public static class ObservableExtensions {
        public static T WithListener<T, TValue>(this T host, Expression<Func<T, TValue>> expression, Action<TValue> listener) where T : IObservableHost {
            var name = expression.GetPropertyNameOrThrow();

            return WithListener(host, name, listener);
        }

        public static T WithoutListener<T, TValue>(this T host, Expression<Func<T, TValue>> expression, Action<TValue> listener) where T : IObservableHost {
            var name = expression.GetPropertyNameOrThrow();

            return WithoutListener(host, name, listener);
        }

        public static T WithListener<T, TValue>(this T host, string propertyName, Action<TValue> listener) where T : IObservableHost {
            host.AddCallback(propertyName, listener);
            return host;
        }

        public static T WithoutListener<T, TValue>(this T host, string propertyName, Action<TValue> listener) where T : IObservableHost {
            host.RemoveCallback(propertyName, listener);
            return host;
        }
    }
}