using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Reactive {
    [PublicAPI]
    public class AnimatedValue<T> : INotifyValueChanged<T>, IAnimation, IReactiveModule {
        public AnimatedValue(T initialValue, IValueInterpolator<T> valueInterpolator) {
            _startValue = initialValue;
            _endValue = initialValue;
            _valueInterpolator = valueInterpolator;
            _set = true;
            _progress = 1f;
            _elapsedTime = 0f;
        }

        public T Value {
            get => _endValue;
            set {
                if (_valueInterpolator.Equals(_endValue, value)) {
                    return;
                }

                _startValue = CurrentValue;
                _endValue = value;

                _elapsedTime = 0f;
                _progress = 0f;

                var shouldNotify = !_set;
                _set = false;

                if (shouldNotify) {
                    OnStart?.Invoke(this);
                    AnimationStartedEvent?.Invoke();
                }
            }
        }

        public float Progress {
            get => _progress;
            private set {
                _progress = value;
                ValueChangedEvent?.Invoke(CurrentValue);
            }
        }

        T INotifyValueChanged<T>.Value => CurrentValue;

        public T CurrentValue => _valueInterpolator.Lerp(_startValue, _endValue, _progress);
        public bool IsFinished => _set;

        public AnimationDuration Duration { get; set; }
        public AnimationCurve Curve { get; set; } = AnimationCurve.Linear;

        public Action<AnimatedValue<T>>? OnFinish { get; set; }
        public Action<AnimatedValue<T>>? OnStart { get; set; }

        public event Action? AnimationFinishedEvent;
        public event Action? AnimationStartedEvent;
        public event Action<T>? ValueChangedEvent;

        private readonly IValueInterpolator<T> _valueInterpolator;
        private T _endValue;
        private T _startValue;

        private float _progress;
        private float _elapsedTime;
        private bool _set;

        public void SetValueImmediate(T value, bool silent = false) {
            _set = true;
            _startValue = value;
            _endValue = value;
            _endValue = _valueInterpolator.Lerp(_startValue, _endValue, 1f);

            if (!silent) {
                Progress = 1f;
                FinishAnimation();
            } else {
                _progress = 1f;
            }
        }

        /// <summary>
        /// Evaluates the animation on the next frame. Use this to provide default values to subscribers.
        /// If called when the animation is running, nothing will happen.
        /// </summary>
        public void EvaluateNextFrame() {
            _set = false;
        }

        public void ClearBindings() {
            ValueChangedEvent = null;
        }

        public void FinishToEnd() {
            Progress = 1f;
            FinishAnimation();
        }

        public void Finish() {
            FinishAnimation();
        }

        void IReactiveModule.OnUpdate() {
            if (_set) {
                return;
            }

            if (Duration.Unit is DurationUnit.Seconds) {
                _elapsedTime += Time.deltaTime;
                _progress = Mathf.Clamp01(_elapsedTime / Duration);
            } else {
                _progress = Mathf.Lerp(Progress, 1f, Time.deltaTime * Duration);
            }

            Progress = Curve.Evaluate(Progress);
            // Finishing if needed
            if (Mathf.Approximately(1f, Progress)) {
                FinishAnimation();
            }
        }

        void IReactiveModule.OnBind() { }
        void IReactiveModule.OnUnbind() { }

        private void FinishAnimation() {
            _set = true;
            _elapsedTime = 0f;

            AnimationFinishedEvent?.Invoke();
            OnFinish?.Invoke(this);
        }

        public static implicit operator T(AnimatedValue<T> value) {
            return value.CurrentValue;
        }
    }
}