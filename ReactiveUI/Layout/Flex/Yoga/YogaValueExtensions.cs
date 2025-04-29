using JetBrains.Annotations;

namespace Reactive.Yoga;

// ReSharper disable InconsistentNaming
[PublicAPI]
public static class YogaValueExtensions {
    public static YogaValue pt(this float val) {
        return new YogaValue(val, Unit.Percent);
    }

    public static YogaValue pt(this int val) {
        return new YogaValue(val, Unit.Percent);
    }

    public static YogaValue pct(this float val) {
        return new YogaValue(val, Unit.Percent);
    }

    public static YogaValue pct(this int val) {
        return new YogaValue(val, Unit.Percent);
    }
}