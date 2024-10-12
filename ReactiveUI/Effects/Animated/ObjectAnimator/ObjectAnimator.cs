using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Reactive {
    [PublicAPI]
    public class ObjectAnimator<T> : IObjectAnimator<T>, IReactiveModule {
        public ObjectAnimator(Action<T, float> callback) {
            _callback = callback;
        }

        public AnimationDuration Duration { get; set; }
        public AnimationCurve Curve { get; set; } = AnimationCurve.Exponential;

        public float Progress { get; private set; }
        public bool IsFinished => _isFinished;

        public event Action? AnimationFinishedEvent;

        private readonly Action<T, float> _callback;
        private T? _instance;
        private float _elapsedTime;
        private bool _isFinished = true;

        public void StartAnimation(T instance, bool resetProgress = true) {
            _instance = instance;
            if (resetProgress) {
                _elapsedTime = 0f;
                Progress = 0f;
            }
            _isFinished = false;
            OnUpdate();
        }

        public void StopAnimation(bool finish = false) {
            if (finish) {
                if (_instance != null) {
                    _callback(_instance!, 1f);
                }
                _elapsedTime = 0f;
            }
            _instance = default;
            _isFinished = true;
        }

        public void OnUpdate() {
            if (IsFinished) return;
            _elapsedTime += Time.deltaTime;
            Progress = Mathf.Clamp01(_elapsedTime / Duration);
            Progress = Curve.Evaluate(Progress);
            _callback(_instance!, Progress);
            //finishing
            if (_elapsedTime >= Duration) {
                _isFinished = true;
                _elapsedTime = 0f;
                AnimationFinishedEvent?.Invoke();
            }
        }

        void IReactiveModule.OnDestroy() { }
    }
}