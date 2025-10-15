using UnityEngine;

namespace Reactive {
    public class SingleValueInterpolator : ValueInterpolator<SingleValueInterpolator, float> {
        public override float Lerp(float from, float to, float value) {
            return Mathf.LerpUnclamped(from, to, value);
        }

        public override bool Equals(float x, float y) {
            return Mathf.Approximately(x, y);
        }
    }
}