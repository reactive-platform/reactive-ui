using UnityEngine;

namespace Reactive;

public class BezierCurve(float x1, float y1, float x2, float y2) : AnimationCurve {
    public override float Evaluate(float progress) {
        var t = Solve(progress, 1e-5f, 10);
        
        return Bezier(t, 0f, y1, y2, 1f);
    }

    private static float Bezier(float t, float p0, float p1, float p2, float p3) {
        var c = 1 - t;
        return c * c * c * p0
            + 3 * c * c * t * p1
            + 3 * c * t * t * p2
            + t * t * t * p3;
    }

    private float Solve(float progress, float epsilon, int maxIter) {
        var t = progress;
        
        for (var i = 0; i < maxIter; i++) {
            var x = Bezier(t, 0f, x1, x2, 1f) - progress;
            if (Mathf.Abs(x) < epsilon) break;
            
            var dx = BezierDerivative(t, 0f, x1, x2, 1f);
            if (Mathf.Abs(dx) < 1e-6f) break;
            
            t -= x / dx;
        }
        
        return Mathf.Clamp(t, 0f, 1f);
    }

    private static float BezierDerivative(float t, float p0, float p1, float p2, float p3) {
        var c = 1 - t;
        return 3 * c * c * (p1 - p0)
            + 6 * c * t * (p2 - p1)
            + 3 * t * t * (p3 - p2);
    }
}