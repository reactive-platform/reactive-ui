using UnityEngine;

namespace Reactive {
    public class Vector2ValueInterpolator : ValueInterpolator<Vector2ValueInterpolator, Vector2> {
        public override Vector2 Lerp(Vector2 from, Vector2 to, float value) {
            return Vector2.Lerp(from, to, value);
        }

        public override bool Equals(Vector2 x, Vector2 y) {
            return Mathf.Approximately(x.x, y.x) && Mathf.Approximately(x.y, y.y);
        }
    }
}