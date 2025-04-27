using System;
using JetBrains.Annotations;

namespace Reactive.Yoga {
    [PublicAPI]
    public struct YogaValue : IEquatable<YogaValue> {
        public YogaValue(float value, Unit unit) {
            this.value = value;
            this.unit = unit;
        }

        public static readonly YogaValue Undefined = new(float.NaN, Unit.Undefined);
        public static readonly YogaValue Zero = new(0, Unit.Point);
        public static readonly YogaValue Auto = new(float.NaN, Unit.Auto);
        public static readonly YogaValue MaxContent = new(float.NaN, Unit.MaxContent);
        public static readonly YogaValue FitContent = new(float.NaN, Unit.FitContent);
        public static readonly YogaValue Stretch = new(float.NaN, Unit.Stretch);

        public float value;
        public Unit unit;

        public override string ToString() {
            if (unit is Unit.Undefined) {
                return "undefined";
            }

            if (unit is Unit.Auto) {
                return "auto";
            }
            
            var unt = unit switch {
                Unit.Percent => "%",
                Unit.Point => "pt",
                _ => throw new ArgumentOutOfRangeException()
            };
            
            return $"{value}{unt}";
        }

        public static YogaValue Percent(float value) {
            return new YogaValue(value, Unit.Percent);
        }
        
        public static YogaValue Point(float value) {
            return new YogaValue(value, Unit.Point);
        }

        public static implicit operator YogaValue(float value) {
            return new YogaValue(value, Unit.Point);
        }

        public static implicit operator YogaValue(string str) {
            var unit = Unit.Undefined;
            var value = 0f;
            if (str is "auto") {
                unit = Unit.Auto;
            } else if (str.EndsWith("%")) {
                value = float.Parse(str.Replace("%", ""));
                unit = Unit.Percent;
            } else if (float.TryParse(str, out value)) {
                unit = Unit.Point;
            }
            return new YogaValue(value, unit);
        }

        public static bool operator ==(YogaValue left, YogaValue right) {
            return left.unit == right.unit && (left.unit is Unit.Undefined || Math.Abs(left.value - right.value) < 0.001f);
        }

        public static bool operator !=(YogaValue left, YogaValue right) {
            return !(left == right);
        }

        public bool Equals(YogaValue other) {
            return value.Equals(other.value) && unit == other.unit;
        }

        public override bool Equals(object? obj) {
            return obj is YogaValue other && Equals(other);
        }

        public override int GetHashCode() {
            unchecked {
                return (value.GetHashCode() * 397) ^ (int)unit;
            }
        }
    }
}