using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Reactive {
    [PublicAPI]
    public class AnimatedValue<T> : INotifyValueChanged<T>, IReactiveModule {
        public AnimatedValue(T initialValue, IValueInterpolator<T> valueInterpolator) {
            _startValue = initialValue;
            _endValue = initialValue;
            _valueInterpolator = valueInterpolator;
            _set = false;
            _setThisFrame = true;
            _elapsedTime = 0f;
        }

        public T Value {
            get => _endValue;
            set {
                if (_valueInterpolator.Equals(_endValue, value)) return;
                _startValue = CurrentValue;
                _endValue = value;
                if (Mode is InterpolationMode.TimeDelta) {
                    _progress = 0f;
                } else {
                    _elapsedTime = 0f;
                }
                _set = false;
                _setThisFrame = true;
            }
        }

        public float Progress {
            get => _progress;
            private set {
                _progress = value;
                ValueChangedEvent?.Invoke(CurrentValue);
            }
        }

        public T CurrentValue => _valueInterpolator.Lerp(_startValue, _endValue, _progress);

        public InterpolationMode Mode { get; set; } = InterpolationMode.Curve;
        public float LerpFactor { get; set; } = 10f;

        public AnimationDuration Duration { get; set; }
        public AnimationCurve Curve { get; set; } = AnimationCurve.Exponential;
        public Action<AnimatedValue<T>>? OnFinish { get; set; }

        public event Action<T>? ValueChangedEvent;

        private readonly IValueInterpolator<T> _valueInterpolator;
        private T _endValue;
        private T _startValue;

        private float _progress;
        private float _elapsedTime;

        private bool _setThisFrame;
        private bool _set;

        public void SetValueImmediate(T value) {
            _set = true;
            _startValue = value;
            _endValue = value;
            _endValue = _valueInterpolator.Lerp(_startValue, _endValue, 1f);
            Progress = 1f;
            FinishAnimation();
        }

        public void ClearBindings() {
            ValueChangedEvent = null;
        }

        public void OnUpdate() {
            if (_set) return;
            if (Mode is InterpolationMode.Curve) {
                _elapsedTime += Time.deltaTime;
                Progress = Mathf.Clamp01(_elapsedTime / Duration);
                Progress = Curve.Evaluate(Progress);
                //finishing
                if (_elapsedTime >= Duration) {
                    FinishAnimation();
                }
            } else {
                if (Math.Abs(1f - Progress) < 1e-6) {
                    FinishAnimation();
                    return;
                }
                Progress = Mathf.Lerp(Progress, 1f, Time.deltaTime * LerpFactor);
            }
        }

        private void FinishAnimation() {
            _set = true;
            _elapsedTime = 0f;
            OnFinish?.Invoke(this);
        }

        void IReactiveModule.OnDestroy() { }
    }
}