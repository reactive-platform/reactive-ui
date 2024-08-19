using UnityEngine;

namespace Reactive {
    public class Vector3ValueInterpolator : ValueInterpolator<Vector3> {
        public override Vector3 Lerp(Vector3 from, Vector3 to, float value) {
            return Vector3.Lerp(from, to, value);
        }
    }
}