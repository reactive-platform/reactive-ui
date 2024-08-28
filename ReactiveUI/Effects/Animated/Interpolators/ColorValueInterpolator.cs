using UnityEngine;

namespace Reactive {
    public class ColorValueInterpolator : ValueInterpolator<ColorValueInterpolator, Color> {
        public override Color Lerp(Color from, Color to, float value) {
            return Color.Lerp(from, to, value);
        }

        public override bool Equals(Color x, Color y) {
            return x == y;
        }
    }
}