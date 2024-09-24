using JetBrains.Annotations;

namespace Reactive {
    [PublicAPI]
    public class AnimationCurve {
        /// <summary>
        /// Represents a linear progression. The rate of change is constant,
        /// so the output number will be the same as the input one.
        /// </summary>
        /// <code>
        /// curve.Evaluate(0.2f); // returns 0.2f
        /// curve.Evaluate(0.5f); // returns 0.5f
        /// curve.Evaluate(0.8f); // returns 0.8f
        /// </code>
        public static readonly AnimationCurve Linear = new();
        
        /// <summary>
        /// An exponential curve where the change is initially slow but
        /// rapidly accelerates as progress increases.
        /// </summary>
        /// <code>
        /// curve.Evaluate(0.2f); // returns 0.18f
        /// curve.Evaluate(0.5f); // returns 0.39f
        /// curve.Evaluate(0.8f); // returns 0.55f
        /// </code>
        public static readonly AnimationCurve Exponential = new ExponentialCurve();
        
        /// <summary>
        /// Represents an ease-in curve, where the interpolation starts slowly and accelerates
        /// as progress increases.
        /// </summary>
        /// <code>
        /// curve.Evaluate(0.2f); // returns 0.04f
        /// curve.Evaluate(0.5f); // returns 0.25f
        /// curve.Evaluate(0.8f); // returns 0.64f
        /// </code>
        public static readonly AnimationCurve EaseIn = new EaseInCurve();
        
        /// <summary>
        /// Represents an ease-out curve, where the interpolation starts quickly and
        /// decelerates as it progresses.
        /// </summary>
        /// <code>
        /// curve.Evaluate(0.2f); // returns 0.36f
        /// curve.Evaluate(0.5f); // returns 0.75f
        /// curve.Evaluate(0.8f); // returns 0.96f
        /// </code>
        public static readonly AnimationCurve EaseOut = new EaseOutCurve();
        
        /// <summary>
        /// Represents an ease-in-out curve, which combines the effects of ease-in and ease-out.
        /// It starts slowly, accelerates in the middle, and decelerates toward the end.
        /// </summary>
        /// <code>
        /// curve.Evaluate(0.2f); // returns 0.08f
        /// curve.Evaluate(0.5f); // returns 0.5f
        /// curve.Evaluate(0.8f); // returns 0.92f
        /// </code>
        public static readonly AnimationCurve EaseInOut = new EaseInOutCurve();

        public virtual float Evaluate(float progress) => progress;
    }
}