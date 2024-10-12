using System;

namespace Reactive {
    public struct AnimationDuration {
        /// <summary>
        /// Represents duration of the animation in seconds.
        /// </summary>
        public float Duration;

        private static Exception CreateInvalidFormatException() {
            return new FormatException("The input string was not in correct format. It must be a number with a time suffix");
        }

        private static AnimationDuration FromNumber(string str, float multiplier, int suffixLength = 1) {
            str = str.Remove(str.Length - suffixLength, suffixLength);
            if (!float.TryParse(str, out var num)) {
                throw CreateInvalidFormatException();
            }
            return new AnimationDuration {
                Duration = num * multiplier
            };
        }

        public static implicit operator AnimationDuration(string str) {
            if (str.EndsWith("ms")) {
                return FromNumber(str, 0.001f, 2);
            }
            if (str.EndsWith("s")) {
                return FromNumber(str, 1);
            }
            throw CreateInvalidFormatException();
        }

        public static implicit operator AnimationDuration(float seconds) {
            return new AnimationDuration { Duration = seconds };
        }

        public static implicit operator float(AnimationDuration duration) {
            return duration.Duration;
        }
    }
}