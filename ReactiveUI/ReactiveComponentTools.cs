using System;
using UnityEngine;

namespace Reactive;

public partial class ReactiveComponentBase {
    #region Remember

    protected static ObservableValue<TValue> Remember<TValue>(TValue initialValue) {
        return new ObservableValue<TValue>(initialValue);
    }

    protected AnimatedValue<Color> RememberAnimated(
        Color initialValue,
        AnimationDuration animationDuration,
        float lerpFactor = 10f,
        Optional<AnimationCurve> curve = default,
        Action<AnimatedValue<Color>>? onFinish = null
    ) {
        return ValueUtils.RememberAnimatedColor(this, initialValue, animationDuration, lerpFactor, curve, onFinish);
    }

    protected AnimatedValue<Vector2> RememberAnimated(
        Vector2 initialValue,
        AnimationDuration animationDuration,
        float lerpFactor = 10f,
        Optional<AnimationCurve> curve = default,
        Action<AnimatedValue<Vector2>>? onFinish = null
    ) {
        return ValueUtils.RememberAnimatedVector(this, initialValue, animationDuration, lerpFactor, curve, onFinish);
    }

    protected AnimatedValue<Vector3> RememberAnimated(
        Vector3 initialValue,
        AnimationDuration animationDuration,
        float lerpFactor = 10f,
        Optional<AnimationCurve> curve = default,
        Action<AnimatedValue<Vector3>>? onFinish = null
    ) {
        return ValueUtils.RememberAnimatedVector(this, initialValue, animationDuration, lerpFactor, curve, onFinish);
    }

    protected AnimatedValue<float> RememberAnimated(
        float initialValue,
        AnimationDuration animationDuration,
        float lerpFactor = 10f,
        Optional<AnimationCurve> curve = default,
        Action<AnimatedValue<float>>? onFinish = null
    ) {
        return ValueUtils.RememberAnimatedFloat(this, initialValue, animationDuration, lerpFactor, curve, onFinish);
    }

    protected AnimatedValue<TValue> RememberAnimated<TValue>(
        TValue initialValue,
        IValueInterpolator<TValue> interpolator,
        AnimationDuration duration,
        float lerpFactor = 10f,
        Optional<AnimationCurve> curve = default,
        Action<AnimatedValue<TValue>>? onFinish = null
    ) {
        return ValueUtils.RememberAnimated(this, initialValue, interpolator, duration, lerpFactor, curve, onFinish);
    }

    #endregion
    
    #region Animate
    
    protected IObjectAnimator<T> Animate<T>(
        Action<T, float> callback,
        AnimationDuration duration,
        AnimationCurve? curve = null
    ) {
        return ValueUtils.Animate(this, callback, duration, curve);
    }
    
    #endregion
}