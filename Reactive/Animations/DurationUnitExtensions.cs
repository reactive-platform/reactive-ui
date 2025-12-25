using JetBrains.Annotations;

namespace Reactive;

// ReSharper disable InconsistentNaming
[PublicAPI]
public static class DurationUnitExtensions {
    extension(float t) {
        public AnimationDuration sec => new(t, DurationUnit.Seconds);
        public AnimationDuration ms => new(t / 1000f, DurationUnit.Seconds);
        public AnimationDuration fact => new(t, DurationUnit.TimeDeltaFactor);
    }
    
    extension(int t) {
        public AnimationDuration sec => new(t, DurationUnit.Seconds);
        public AnimationDuration ms => new(t / 1000f, DurationUnit.Seconds);
        public AnimationDuration fact => new(t, DurationUnit.TimeDeltaFactor);
    }
}