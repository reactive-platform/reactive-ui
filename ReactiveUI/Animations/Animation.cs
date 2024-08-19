using System;
using JetBrains.Annotations;
using UnityEngine;
using static Reactive.AnimationDirection;

namespace Reactive {
    [PublicAPI]
    public class Animation<T> : IAnimation, IDisposable {
        public Animation(T target, IAnimationTimeline<T> timeline, string? propertyName) {
            _target = target;
            _timeline = timeline;
            PropertyName = propertyName;
        }

        public AnimationRepeats Repeats { get; init; }
        public AnimationDuration Duration { get; init; }
        public AnimationCurve Curve { get; init; }
        public AnimationDirection Direction { get; init; }
        public bool IsFinished { get; private set; }

        public string? PropertyName { get; }
        public object? Target => _target;

        private readonly T _target;
        private readonly IAnimationTimeline<T> _timeline;

        private int _cycleCount;
        private float _timeElapsed;

        public void Evaluate(float delta) {
            if (IsFinished) return;
            _timeElapsed += delta;
            //
            var progress = Mathf.Clamp01(_timeElapsed / Duration);
            //inverting if reversed
            if (Direction is Reversed or ReversedAlternate) {
                progress = 1 - progress;
            }
            //inverting in case the animation is alternate
            if (Direction is Alternate or ReversedAlternate && _cycleCount % 2 != 0) {
                progress = 1 - progress;
            }
            //applying value
            progress = Curve.Evaluate(progress);
            _timeline.Evaluate(progress, _target);
            //checking for cycle finish
            if (_timeElapsed < Duration) return;
            _cycleCount++;
            _timeElapsed = 0;
            if (!Repeats.Endless && _cycleCount == Repeats.Count) {
                Dispose();
            }
        }

        public void Reset() {
            _timeElapsed = 0;
            _cycleCount = 0;
            IsFinished = false;
        }

        public void Dispose() {
            IsFinished = true;
        }

        public bool Equals(IAnimation other) {
            return ReferenceEquals(this, other) || other.GetHashCode() == GetHashCode();
        }

        public override int GetHashCode() {
            return PropertyName?.GetHashCode() ?? 0;
        }
    }
}