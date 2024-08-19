using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using UnityEngine;

namespace Reactive {
    [PublicAPI]
    public abstract class TransitionBase<T, TValue> : IAnimation {
        protected TransitionBase(
            T target,
            Expression<Func<T, TValue>> expression,
            ValueInterpolator<TValue> interpolator
        ) {
            Target = target;
            PropertyName = expression.GetPropertyNameOrThrow();
            Setter = expression.GeneratePropertySetter();
            Getter = expression.GeneratePropertyGetter();
            Interpolator = interpolator;
            Reset();
        }
        
        protected TransitionBase(
            T target,
            Action<T, TValue> setter,
            Func<T, TValue> getter,
            ValueInterpolator<TValue> interpolator
        )  {
            Target = target;
            Setter = setter;
            Getter = getter;
            Interpolator = interpolator;
            Reset();
        }

        public AnimationDuration Duration { get; init; }
        public AnimationCurve Curve { get; init; }
        public bool IsFinished { get; private set; }

        public string? PropertyName { get; protected init; }

        object? IAnimation.Target => Target;

        private TValue Value {
            get => Getter(Target);
            set => Setter(Target, value);
        }

        protected abstract TValue TargetValue { get; }

        protected readonly T Target;
        protected readonly ValueInterpolator<TValue> Interpolator;
        protected readonly Action<T, TValue> Setter;
        protected readonly Func<T, TValue> Getter;

        private TValue _initialValue = default!;
        private float _timeElapsed;
        private bool _firstEvaluation;

        public void Evaluate(float delta) {
            if (IsFinished) return;
            if (_firstEvaluation) {
                _initialValue = Value;
                _firstEvaluation = false;
            }
            _timeElapsed += delta;
            //applying value
            var progress = Mathf.Clamp01(_timeElapsed / Duration);
            progress = Curve.Evaluate(progress);
            Value = Interpolator.Lerp(_initialValue, TargetValue, progress);
            //checking for finish
            if (_timeElapsed < Duration) return;
            _timeElapsed = 0;
            IsFinished = true;
        }

        public void Reset() {
            _timeElapsed = 0;
            _firstEvaluation = true;
            IsFinished = false;
        }
    }
}