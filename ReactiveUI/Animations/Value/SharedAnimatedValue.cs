using JetBrains.Annotations;

namespace Reactive;

[PublicAPI]
public class SharedAnimatedValue<T>(
    T initialValue,
    IValueInterpolator<T> valueInterpolator
) : AnimatedValue<T>(initialValue, valueInterpolator), ISharedAnimation {
    public void Play() { }
}