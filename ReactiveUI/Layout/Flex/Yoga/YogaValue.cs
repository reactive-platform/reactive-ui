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
            return unit switch {
                Unit.Undefined => "undefined",
                Unit.Auto => "auto",
                Unit.FitContent => "fit-content",
                Unit.MaxContent => "max-content",
                Unit.Stretch => "stretch",

                _ => unit switch {
                    Unit.Percent => $"{value}%",
                    Unit.Point => $"{value}pt",
                    _ => throw new ArgumentOutOfRangeException()
                }
            };
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
            Unit unit;
            float value = 0;

            switch (str) {
                case "undefined":
                    unit = Unit.Undefined;
                    break;
                
                case "auto":
                    unit = Unit.Auto;
                    break;
                
                case "fit-content":
                    unit = Unit.FitContent;
                    break;
                
                case "max-content":
                    unit = Unit.MaxContent;
                    break;
                
                case "stretch":
                    unit = Unit.Stretch;
                    break;

                default: {
                    if (str.EndsWith("%")) {
                        value = float.Parse(str.Replace("%", ""));
                        unit = Unit.Percent;
                        break;
                    }

                    if (str.EndsWith("pt")) {
                        value = float.Parse(str.Replace("pt", ""));
                        unit = Unit.Point;
                        break;
                    }
                    
                    throw new ArgumentOutOfRangeException(nameof(str));
                }
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