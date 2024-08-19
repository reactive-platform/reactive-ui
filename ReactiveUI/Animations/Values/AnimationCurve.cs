using System;
using Curve = UnityEngine.AnimationCurve;

namespace Reactive {
    public struct AnimationCurve {
        private Curve? _curve;

        public float Evaluate(float value) {
            return _curve?.Evaluate(value) ?? value;
        }

        public static implicit operator AnimationCurve(string str) {
            var basicCurve = str switch {
                "ease-in-out" => AnimationBasicCurve.EaseInOut,
                "linear" => AnimationBasicCurve.Linear,
                _ => throw new FormatException("Curve string must be either ease-in-out or linear")
            };
            return basicCurve;
        }

        public static implicit operator AnimationCurve(AnimationBasicCurve basicCurve) {
            var curve = basicCurve switch {
                AnimationBasicCurve.EaseInOut => Curve.EaseInOut(0, 0, 1, 1),
                AnimationBasicCurve.Linear => Curve.Linear(0, 0, 1, 1),
                _ => throw new ArgumentOutOfRangeException()
            };
            return new AnimationCurve { _curve = curve };
        }
    }
}