using JetBrains.Annotations;

namespace Reactive {
    /// <summary>
    /// A struct that provides an optional value. Can represent undefined, null and some.
    /// </summary>
    /// <typeparam name="T">A type of optional value.</typeparam>
    [PublicAPI]
    public struct Optional<T> {
        public T? Value { get; private set; }
        public bool HasValue { get; private set; }

        public void SetValueIfNotSet(T newValue) {
            if (HasValue) return;
            Value = newValue;
            HasValue = true;
        }

        public T GetValueOrDefault(T defaultValue) {
            return HasValue ? Value! : defaultValue;
        }

        public static Optional<T> Some(T? value) {
            return new() {
                Value = value,
                HasValue = true
            };
        }

        public static Optional<T> None() {
            return default;
        }

        public static implicit operator bool(Optional<T> value) {
            return value.HasValue;
        }

        public static implicit operator T?(Optional<T> value) {
            return value.Value;
        }

        public static implicit operator Optional<T>(T? value) {
            return Some(value);
        }
    }
}