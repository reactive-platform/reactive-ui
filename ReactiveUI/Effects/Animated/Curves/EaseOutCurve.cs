using UnityEngine;

namespace Reactive;

public class EaseOutCurve : AnimationCurve {
    public override float Evaluate(float progress) {
        return 1f - Mathf.Pow(1f - progress, 2f);
    }
}