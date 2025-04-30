using System;
using JetBrains.Annotations;

namespace Reactive.Yoga {
    [PublicAPI]
    public struct YogaFrame {
        public static readonly YogaFrame Undefined = new(YogaValue.Undefined);
        public static readonly YogaFrame Zero = new(YogaValue.Zero);
        public static readonly YogaFrame Auto = new(YogaValue.Auto);
        public static readonly YogaFrame MaxContent = new(YogaValue.MaxContent);
        public static readonly YogaFrame FitContent = new(YogaValue.FitContent);
        public static readonly YogaFrame Stretch = new(YogaValue.Stretch);

        public YogaFrame(
            YogaValue top,
            YogaValue bottom,
            YogaValue left,
            YogaValue right
        ) {
            this.top = top;
            this.bottom = bottom;
            this.left = left;
            this.right = right;
        }

        public YogaFrame(YogaValue vertical, YogaValue horizontal) {
            top = vertical;
            bottom = vertical;
            left = horizontal;
            right = horizontal;
        }

        public YogaFrame(YogaValue all) {
            top = all;
            bottom = all;
            left = all;
            right = all;
        }

        public YogaValue this[int idx] {
            get {
                return idx switch {
                    0 => top,
                    1 => right,
                    2 => bottom,
                    3 => left,
                    _ => throw new IndexOutOfRangeException()
                };
            }
        }

        public YogaValue top;
        public YogaValue bottom;
        public YogaValue left;
        public YogaValue right;

        public static implicit operator YogaFrame(string value) {
            return new YogaFrame(value);
        }
        
        public static implicit operator YogaFrame(float value) {
            return new YogaFrame(value);
        }
        
        public static implicit operator YogaFrame(YogaValue value) {
            return new YogaFrame(value);
        }
    }
}