using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Reactive {
    [PublicAPI]
    public static class ObservableExtensions {
        public static T WithListener<T, TValue>(
            this T host,
            Expression<Func<T, TValue>> expression,
            Action<TValue> listener
        ) where T : IObservableHost {
            var name = expression.GetPropertyNameOrThrow();
            host.AddCallback(name, listener);
            return host;
        }
        
        public static T WithoutListener<T, TValue>(
            this T host,
            Expression<Func<T, TValue>> expression,
            Action<TValue> listener
        ) where T : IObservableHost {
            var name = expression.GetPropertyNameOrThrow();
            host.RemoveCallback(name, listener);
            return host;
        }
    }
}