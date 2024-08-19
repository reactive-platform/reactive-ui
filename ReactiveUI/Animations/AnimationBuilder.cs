using JetBrains.Annotations;

namespace Reactive {
    public struct AnimationBuilder<T> {
        public T value;
        public ComponentState state;
        public AnimationDuration duration;
        public AnimationCurve curve;

        public AnimationBuilder<T> For(AnimationDuration duration) {
            this.duration = duration;
            return this;
        }

        public AnimationBuilder<T> By(AnimationCurve curve) {
            this.curve = curve;
            return this;
        }
    }

    [PublicAPI]
    public static class AnimationBuilderExtensions {
        public static AnimationBuilder<T> When<T>(this T value, ComponentState state) {
            return new AnimationBuilder<T> {
                value = value,
                state = state,
                curve = AnimationBasicCurve.EaseInOut
            };
        }
    }
}