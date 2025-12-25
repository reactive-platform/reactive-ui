using JetBrains.Annotations;

namespace Reactive.Yoga;

// ReSharper disable InconsistentNaming
[PublicAPI]
public static class YogaValueExtensions {
    extension(float f) {
        public YogaValue pt => new(f, Unit.Point);
        public YogaValue pct => new(f, Unit.Percent);
    }
    
    extension(int f) {
        public YogaValue pt => new(f, Unit.Point);
        public YogaValue pct => new(f, Unit.Percent);
    }
}