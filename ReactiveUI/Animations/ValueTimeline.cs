using System;
using JetBrains.Annotations;

namespace Reactive {
    [PublicAPI]
    public class ValueTimeline<T, TValue> : IAnimationTimeline<T> {
        public ValueTimeline(Action<T, TValue> applicator, ValueInterpolator<TValue> interpolator) {
            _applicator = applicator;
            _interpolator = interpolator;
        }

        public TValue FromValue {
            get {
                ThrowIfInvalid();
                return _fromValue!;
            }
            init => _fromValue = value;
        }

        public TValue ToValue {
            get {
                ThrowIfInvalid();
                return _toValue!;
            }
            init => _toValue = value;
        }

        private readonly Action<T, TValue> _applicator;
        private readonly ValueInterpolator<TValue> _interpolator;
        private readonly TValue? _fromValue;
        private readonly TValue? _toValue;
        private bool? _valuesValid;

        public void Evaluate(float value, T target) {
            var val = _interpolator.Lerp(FromValue, ToValue, value);
            _applicator(target, val);
        }

        private void ValidateValues() {
            if (_valuesValid.HasValue) return;
            _valuesValid = _fromValue != null && _toValue != null;
        }

        private void ThrowIfInvalid() {
            ValidateValues();
            if (!_valuesValid.GetValueOrDefault()) {
                throw new InvalidOperationException("Values must be initialized");
            }
        }
    }
}