using System;

namespace Reactive {
    public struct AnimationRepeats {
        /// <summary>
        /// Represents a count of repeats.
        /// </summary>
        public int Count;

        /// <summary>
        /// Represents is animation endless or not.
        /// </summary>
        public bool Endless => Count == -1;

        public static implicit operator AnimationRepeats(string str) {
            if (int.TryParse(str, out var count)) {
                return new() { Count = count };
            } else if (str is "endless") {
                return new() { Count = -1 };
            } else {
                throw new ArgumentException("The value can be either a number or an endless keyword");
            }
        }

        public static implicit operator AnimationRepeats(int count) {
            return new AnimationRepeats { Count = count };
        }
    }
}