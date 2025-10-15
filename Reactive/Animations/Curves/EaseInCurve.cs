using UnityEngine;

namespace Reactive {
    public class EaseInCurve : AnimationCurve {
        public override float Evaluate(float progress) {
            return Mathf.Pow(progress, 2f);
        }
    }
}
