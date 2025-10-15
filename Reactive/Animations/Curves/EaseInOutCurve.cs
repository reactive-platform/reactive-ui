using UnityEngine;

namespace Reactive {
    public class EaseInOutCurve : AnimationCurve {
        public override float Evaluate(float progress) {
            return progress < 0.5f
                ? 2f * progress * progress
                : 1f - Mathf.Pow(-2f * progress + 2f, 2f) / 2f;
        }
    }
}