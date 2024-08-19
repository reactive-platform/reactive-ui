using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using UnityEngine;

namespace Reactive {
    [PublicAPI]
    public class Transition<T, TValue> : TransitionBase<T, TValue> {
        public Transition(
            T target,
            Expression<Func<T, TValue>> expression,
            TValue targetValue,
            ValueInterpolator<TValue> interpolator
        ) : base(
            target,
            expression,
            interpolator
        ) {
            _targetValue = targetValue;
            Reset();
        }

        private Transition(
            T target,
            Action<T, TValue> setter,
            Func<T, TValue> getter,
            TValue targetValue,
            ValueInterpolator<TValue> interpolator
        ) : base(
            target,
            setter,
            getter,
            interpolator
        ) {
            _targetValue = targetValue;
        }

        protected override TValue TargetValue => _targetValue;

        private readonly TValue _targetValue;

        public Transition<T, TValue> CreateCopy(Optional<TValue> targetValue = default) {
            return new Transition<T, TValue>(
                Target,
                Setter,
                Getter,
                targetValue.GetValueOrDefault(_targetValue),
                Interpolator
            ) {
                PropertyName = PropertyName,
                Duration = Duration
            };
        }
    }
}