using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Reactive {
    [PublicAPI]
    public abstract class ValueInterpolator {
        public abstract Type Type { get; }

        private static readonly List<ValueInterpolator> basicInterpolators = new() {
            new ColorValueInterpolator(),
            new SingleValueInterpolator(),
            new Vector3ValueInterpolator(),
            new Vector2ValueInterpolator()
        };
        
        private static readonly Dictionary<Type, ValueInterpolator> cachedInterpolators = new();

        public static ValueInterpolator<T> GetInterpolator<T>() {
            var type = typeof(T);
            if (!cachedInterpolators.TryGetValue(type, out var value)) {
                foreach (var interpolator in basicInterpolators) {
                    if (interpolator.Type != type) continue;
                    value = interpolator;
                    break;
                }
                if (value == null) {
                    throw new KeyNotFoundException($"Interpolator for {type} does not exist");
                }
                cachedInterpolators[type] = value;
            }
            return (ValueInterpolator<T>)value;
        }

        public static void AddInterpolator<T>(ValueInterpolator<T> interpolator) {
            cachedInterpolators[interpolator.Type] = interpolator;
        }

        public static void RemoveInterpolator<T>(ValueInterpolator<T> interpolator) {
            cachedInterpolators.Remove(interpolator.Type);
        }
    }

    [PublicAPI]
    public abstract class ValueInterpolator<T> : ValueInterpolator {
        public sealed override Type Type => typeof(T);

        public abstract T Lerp(T from, T to, float value);
    }
}