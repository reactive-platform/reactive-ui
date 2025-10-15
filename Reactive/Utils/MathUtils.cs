using JetBrains.Annotations;
using UnityEngine;

namespace Reactive;

[PublicAPI]
public static class MathUtils {
    public static float Map(float val, float inMin, float inMax, float outMin, float outMax) {
        return (val - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
    
    public static float RoundStepped(float value, float step, float startValue = 0) {
        if (step is 0) return value;
        var relativeValue = value - startValue;
        return startValue + Mathf.Round(relativeValue / step) * step;
    }
}