using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Reactive {
    [PublicAPI]
    public class TransitionBuilder<TValue> {
        public ComponentState State { get; private set; }
        public TValue? Value { get; private set; }
        public AnimationDuration Duration { get; private set; }
        public AnimationCurve Curve { get; private set; }

        public TransitionBuilder<TValue> WithValue(TValue value) {
            Value = value;
            return this;
        }

        public TransitionBuilder<TValue> WithState(ComponentState state) {
            State = state;
            return this;
        }

        public TransitionBuilder<TValue> WithDuration(AnimationDuration duration) {
            Duration = duration;
            return this;
        }

        public TransitionBuilder<TValue> WithCurve(AnimationCurve curve) {
            Curve = curve;
            return this;
        }

        public Transition<T, TValue> Build<T>(T target, Expression<Func<T, TValue>> expression) {
            if (Value == null) {
                throw new InvalidOperationException("Value is null");
            }
            var interpolator = ValueInterpolator.GetInterpolator<TValue>();
            return new Transition<T, TValue>(target, expression, Value, interpolator) {
                Duration = Duration,
                Curve = Curve
            };
        }
    }
}