﻿using System;
using JetBrains.Annotations;

namespace Reactive.Yoga {
    [PublicAPI]
    public struct YogaVector : IEquatable<YogaVector> {
        public static readonly YogaVector Auto = new() {
            x = YogaValue.Auto,
            y = YogaValue.Auto
        };
        
        public static readonly YogaVector Undefined = new() {
            x = YogaValue.Undefined,
            y = YogaValue.Undefined
        };
        
        public YogaVector(YogaValue x, YogaValue y) {
            this.x = x;
            this.y = y;
        }
        
        public YogaVector(YogaValue value) {
            x = value;
            y = value;
        }

        public YogaValue this[int idx] {
            get {
                return idx switch {
                    0 => x,
                    1 => y,
                    _ => throw new IndexOutOfRangeException()
                };
            }
        }

        public YogaValue x;
        public YogaValue y;

        public static implicit operator YogaVector(string value) {
            return new() { x = value, y = value };
        }
        
        public static implicit operator YogaVector(float value) {
            return new(value);
        }
        
        public static bool operator ==(YogaVector left, YogaVector right) {
            return left.x == right.x && left.y == right.y;
        }
        
        public static bool operator !=(YogaVector left, YogaVector right) {
            return left.x != right.x || left.y != right.y;
        }
        
        public bool Equals(YogaVector other) {
            return x == other.x && y.Equals(other.y);
        }

        public override bool Equals(object? obj) {
            return obj is YogaVector other && Equals(other);
        }
        
        public override int GetHashCode() {
            unchecked {
                return (x.GetHashCode() * 397) ^ y.GetHashCode();
            }
        }
    }
}