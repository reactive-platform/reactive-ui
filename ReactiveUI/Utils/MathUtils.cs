using JetBrains.Annotations;

namespace Reactive;

[PublicAPI]
public static class MathUtils {
    public static float Map(float val, float inMin, float inMax, float outMin, float outMax) {
        return (val - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}