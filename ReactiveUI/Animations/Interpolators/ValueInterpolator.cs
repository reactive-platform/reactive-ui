using JetBrains.Annotations;

namespace Reactive {
    [PublicAPI]
    public abstract class ValueInterpolator<T, TValue> : IValueInterpolator<TValue> where T : ValueInterpolator<T, TValue>, new() {
        public static T Instance { get; } = new();
        
        public abstract TValue Lerp(TValue from, TValue to, float value);
        public abstract bool Equals(TValue x, TValue y);

        public int GetHashCode(TValue obj) {
            return obj!.GetHashCode();
        }
    }
}