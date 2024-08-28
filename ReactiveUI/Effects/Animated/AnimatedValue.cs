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
                _elapsedTime = 0f;
                _endValue = value;
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

        public AnimationDuration Duration { get; set; }
        public Action<AnimatedValue<T>>? OnFinish { get; set; }

        public event Action<T>? ValueChangedEvent;

        private readonly IValueInterpolator<T> _valueInterpolator;
        private T _endValue;
        private T _startValue;

        private float _progress;
        private float _elapsedTime;

        private bool _setThisFrame;
        private bool _set;

        public void ClearBindings() {
            ValueChangedEvent = null;
        }

        public void OnUpdate() {
            if (_set) return;
            _elapsedTime += Time.deltaTime;
            Progress = Mathf.Clamp01(_elapsedTime / Duration);
            //finishing
            if (_elapsedTime >= Duration) {
                _set = true;
                _elapsedTime = 0f;
                OnFinish?.Invoke(this);
            }
        }

        void IReactiveModule.OnDestroy() { }
    }
}