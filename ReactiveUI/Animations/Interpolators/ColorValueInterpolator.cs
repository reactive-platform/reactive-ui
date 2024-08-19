using UnityEngine;

namespace Reactive {
    public class ColorValueInterpolator : ValueInterpolator<Color> {
        public override Color Lerp(Color from, Color to, float value) {
            return Color.Lerp(from, to, value);
        }
    }
}