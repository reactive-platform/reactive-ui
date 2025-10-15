using UnityEngine;

namespace Reactive {
    public class ExponentialCurve : AnimationCurve {
        private static readonly float infimum = 1f - Mathf.Exp(-0f);
        private static readonly float supremum = 1f - Mathf.Exp(-1f);

        public override float Evaluate(float progress) {
            return MathUtils.Map(
                1f - Mathf.Exp(-progress),
                infimum,
                supremum,
                0f,
                1f
            );
        }
    }
}