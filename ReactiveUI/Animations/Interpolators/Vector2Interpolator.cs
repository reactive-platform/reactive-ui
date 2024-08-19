using UnityEngine;

namespace Reactive {
    public class Vector2ValueInterpolator : ValueInterpolator<Vector2> {
        public override Vector2 Lerp(Vector2 from, Vector2 to, float value) {
            return Vector2.Lerp(from, to, value);
        }
    }
}