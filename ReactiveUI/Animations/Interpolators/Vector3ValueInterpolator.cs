using UnityEngine;

namespace Reactive {
    public class Vector3ValueInterpolator : ValueInterpolator<Vector3ValueInterpolator, Vector3> {
        public override Vector3 Lerp(Vector3 from, Vector3 to, float value) {
            return Vector3.Lerp(from, to, value);
        }

        public override bool Equals(Vector3 x, Vector3 y) {
            return Mathf.Approximately(x.x, y.x) &&
                Mathf.Approximately(x.y, y.y) && 
                Mathf.Approximately(x.z, y.z);
        }
    }
}