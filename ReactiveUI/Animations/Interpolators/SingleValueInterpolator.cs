using UnityEngine;

namespace Reactive {
    public class SingleValueInterpolator : ValueInterpolator<float> {
        public override float Lerp(float from, float to, float value) {
            return Mathf.Lerp(from, to, value);
        }
    }
}