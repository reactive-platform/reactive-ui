namespace Reactive {
    public struct AnimationDuration {
        /// <summary>
        /// Represents duration of the animation either as seconds or multiplier.
        /// </summary>
        public float Value;

        /// <summary>
        /// Represents unit of the value.
        /// </summary>
        public DurationUnit Unit;
        
        public static implicit operator float(AnimationDuration duration) {
            return duration.Value;
        }
    }
}