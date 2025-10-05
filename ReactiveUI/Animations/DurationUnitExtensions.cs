using JetBrains.Annotations;

namespace Reactive;

// ReSharper disable InconsistentNaming
[PublicAPI]
public static class DurationUnitExtensions {
    public static AnimationDuration sec(this float time) {
        return new AnimationDuration { Value = time, Unit = DurationUnit.Seconds };
    }

    public static AnimationDuration sec(this int time) {
        return new AnimationDuration { Value = time, Unit = DurationUnit.Seconds };
    }

    public static AnimationDuration ms(this float time) {
        return new AnimationDuration { Value = time / 1000, Unit = DurationUnit.Seconds };
    }

    public static AnimationDuration ms(this int time) {
        return new AnimationDuration { Value = time / 1000f, Unit = DurationUnit.Seconds };
    }

    public static AnimationDuration fact(this float factor) {
        return new AnimationDuration { Value = factor, Unit = DurationUnit.TimeDeltaFactor };
    }
    
    public static AnimationDuration fact(this int factor) {
        return new AnimationDuration { Value = factor, Unit = DurationUnit.TimeDeltaFactor };
    }
}