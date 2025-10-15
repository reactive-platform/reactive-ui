using JetBrains.Annotations;

namespace Reactive {
    [PublicAPI]
    public class AnimationCurve {
        /// <summary>
        /// Linear curve.
        /// Changes at a constant rate, producing uniform motion
        /// without any acceleration or deceleration.
        /// </summary>
        public static readonly AnimationCurve Linear = new();

        /// <summary>
        /// Exponential curve.
        /// Starts slowly and accelerates rapidly toward the end,
        /// producing a sharp, dynamic motion.
        /// </summary>
        public static readonly AnimationCurve Exponential = new ExponentialCurve();

        /// <summary>
        /// Ease-in curve.
        /// Begins slowly and accelerates over time,
        /// creating a natural “slow-start” motion.
        /// </summary>
        public static readonly AnimationCurve EaseIn = new EaseInCurve();

        /// <summary>
        /// Ease-out curve.
        /// Starts quickly and decelerates smoothly toward the end,
        /// creating a natural “slow-finish” motion.
        /// </summary>
        public static readonly AnimationCurve EaseOut = new EaseOutCurve();

        /// <summary>
        /// Ease-in-out curve.
        /// S-shaped motion: slow at the start, faster in the middle,
        /// and slow again at the end, providing smooth acceleration and deceleration.
        /// </summary>
        public static readonly AnimationCurve EaseInOut = new EaseInOutCurve();

        /// <summary>
        /// Quintic ease-in curve.
        /// Starts extremely slowly and then accelerates strongly toward the end,
        /// producing a dramatic “slow-start, fast-finish” motion.
        /// </summary>
        public static readonly AnimationCurve EaseInQuint = new BezierCurve(0.64f, 0.0f, 0.78f, 0.0f);

        /// <summary>
        /// Quintic ease-out curve.
        /// Begins very fast and decelerates smoothly, creating a strong
        /// “snap into place” effect at the end.
        /// </summary>
        public static readonly AnimationCurve EaseOutQuint = new BezierCurve(0.22f, 1.0f, 0.36f, 1.0f);

        /// <summary>
        /// Quintic ease-in-out curve.
        /// Symmetric S-shaped motion: very slow at the start,
        /// rapid through the middle, and very slow at the end.
        /// </summary>
        public static readonly AnimationCurve EaseInOutQuint = new BezierCurve(0.83f, 0.0f, 0.17f, 1.0f);

        /// <summary>
        /// Exponential ease-in curve.
        /// Starts almost flat and quickly ramps up,
        /// ideal for effects that need a long anticipation followed by a sudden move.
        /// </summary>
        public static readonly AnimationCurve EaseInExpo = new BezierCurve(0.95f, 0.05f, 0.795f, 0.035f);

        /// <summary>
        /// Exponential ease-out curve.
        /// Launches very quickly then glides to the target,
        /// great for UI elements that should feel snappy but gentle when stopping.
        /// </summary>
        public static readonly AnimationCurve EaseOutExpo = new BezierCurve(0.19f, 1.0f, 0.22f, 1.0f);

        /// <summary>
        /// Exponential ease-in-out curve.
        /// Nearly flat at both ends with a steep middle section,
        /// producing a dramatic “linger then burst then settle” effect.
        /// </summary>
        public static readonly AnimationCurve EaseInOutExpo = new BezierCurve(1.0f, 0.0f, 0.0f, 1.0f);

        /// <summary>
        /// Back ease-in curve.
        /// Moves slightly backward before accelerating forward,
        /// giving a subtle anticipation or “wind-up” motion.
        /// </summary>
        public static readonly AnimationCurve EaseInBack = new BezierCurve(0.6f, -0.28f, 0.735f, 0.045f);

        /// <summary>
        /// Back ease-out curve.
        /// Overshoots the target and then settles back,
        /// perfect for playful UI elements or bouncy transitions.
        /// </summary>
        public static readonly AnimationCurve EaseOutBack = new BezierCurve(0.175f, 0.885f, 0.32f, 1.275f);

        /// <summary>
        /// Back ease-in-out curve.
        /// Combines backward anticipation at the start and a slight overshoot at the end,
        /// creating a dramatic “push–pull” feel.
        /// </summary>
        public static readonly AnimationCurve EaseInOutBack = new BezierCurve(0.68f, -0.55f, 0.265f, 1.55f);

        /// <summary>
        /// Sine ease-in curve.
        /// Starts gently and accelerates following a quarter sine wave,
        /// giving a natural, soft ramp-up.
        /// </summary>
        public static readonly AnimationCurve EaseInSine = new BezierCurve(0.47f, 0.0f, 0.745f, 0.715f);

        /// <summary>
        /// Sine ease-out curve.
        /// Decelerates like a quarter sine wave, excellent for smooth stops.
        /// </summary>
        public static readonly AnimationCurve EaseOutSine = new BezierCurve(0.39f, 0.575f, 0.565f, 1.0f);

        /// <summary>
        /// Sine ease-in-out curve.
        /// Symmetric half-sine motion with very smooth start and end.
        /// </summary>
        public static readonly AnimationCurve EaseInOutSine = new BezierCurve(0.445f, 0.05f, 0.55f, 0.95f);

        /// <summary>
        /// Quadratic ease-in curve.
        /// A gentle slow-start that accelerates steadily,
        /// useful when you want subtle acceleration.
        /// </summary>
        public static readonly AnimationCurve EaseInQuad = new BezierCurve(0.55f, 0.085f, 0.68f, 0.53f);

        /// <summary>
        /// Quadratic ease-out curve.
        /// Quick acceleration followed by a soft landing,
        /// smoother than linear without being dramatic.
        /// </summary>
        public static readonly AnimationCurve EaseOutQuad = new BezierCurve(0.25f, 0.46f, 0.45f, 0.94f);

        /// <summary>
        /// Quadratic ease-in-out curve.
        /// Balanced S-curve with mild acceleration and deceleration.
        /// </summary>
        public static readonly AnimationCurve EaseInOutQuad = new BezierCurve(0.455f, 0.03f, 0.515f, 0.955f);

        /// <summary>
        /// Cubic ease-in curve.
        /// More pronounced acceleration than Quad, still smooth at the start.
        /// </summary>
        public static readonly AnimationCurve EaseInCubic = new BezierCurve(0.55f, 0.055f, 0.675f, 0.19f);

        /// <summary>
        /// Cubic ease-out curve.
        /// Strong deceleration with a pleasant glide to the end.
        /// </summary>
        public static readonly AnimationCurve EaseOutCubic = new BezierCurve(0.215f, 0.61f, 0.355f, 1.0f);

        /// <summary>
        /// Cubic ease-in-out curve.
        /// Classic S-curve with a stronger middle acceleration than Quad.
        /// </summary>
        public static readonly AnimationCurve EaseInOutCubic = new BezierCurve(0.645f, 0.045f, 0.355f, 1.0f);

        public virtual float Evaluate(float progress) => progress;
    }
}