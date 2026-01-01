using System;
using System.Collections.Generic;
using UnityEngine;

namespace Reactive;

public partial class ReactiveComponent {
    #region Lerp

    protected static float Lerp(float from, float to, float value) {
        return Mathf.LerpUnclamped(from, to, value);
    }
    
    protected static Color Lerp(Color from, Color to, float value) {
        return Color.LerpUnclamped(from, to, value);
    }
    
    protected static Vector2 Lerp(Vector2 from, Vector2 to, float value) {
        return Vector2.LerpUnclamped(from, to, value);
    }
    
    protected static Vector3 Lerp(Vector3 from, Vector3 to, float value) {
        return Vector3.LerpUnclamped(from, to, value);
    }
    
    protected static Quaternion Lerp(Quaternion from, Quaternion to, float value) {
        return Quaternion.LerpUnclamped(from, to, value);
    }

    #endregion

    #region Owned Values

    protected static ObservableValue<TValue> Remember<TValue>(TValue initialValue) {
        return new ObservableValue<TValue>(initialValue);
    }

    protected AnimatedValue<Color> RememberAnimated(
        Color initialValue,
        AnimationDuration animationDuration,
        AnimationCurve? curve = null,
        Action<AnimatedValue<Color>>? onFinish = null
    ) {
        return ValueUtils.RememberAnimatedColor(this, initialValue, animationDuration, curve, onFinish);
    }

    protected AnimatedValue<Vector2> RememberAnimated(
        Vector2 initialValue,
        AnimationDuration animationDuration,
        AnimationCurve? curve = null,
        Action<AnimatedValue<Vector2>>? onFinish = null
    ) {
        return ValueUtils.RememberAnimatedVector(this, initialValue, animationDuration, curve, onFinish);
    }

    protected AnimatedValue<Vector3> RememberAnimated(
        Vector3 initialValue,
        AnimationDuration animationDuration,
        AnimationCurve? curve = null,
        Action<AnimatedValue<Vector3>>? onFinish = null
    ) {
        return ValueUtils.RememberAnimatedVector(this, initialValue, animationDuration, curve, onFinish);
    }

    protected AnimatedValue<float> RememberAnimated(
        float initialValue,
        AnimationDuration animationDuration,
        AnimationCurve? curve = null,
        Action<AnimatedValue<float>>? onFinish = null
    ) {
        return ValueUtils.RememberAnimatedFloat(this, initialValue, animationDuration, curve, onFinish);
    }

    protected AnimatedValue<TValue> RememberAnimated<TValue>(
        TValue initialValue,
        IValueInterpolator<TValue> interpolator,
        AnimationDuration duration,
        AnimationCurve? curve = null,
        Action<AnimatedValue<TValue>>? onFinish = null
    ) {
        return ValueUtils.RememberAnimated(this, initialValue, interpolator, duration, curve, onFinish);
    }

    #endregion

    #region Unowned Values

    protected static SharedAnimatedValue<float> Animated(
        float initialValue,
        AnimationDuration duration,
        AnimationCurve? curve = null,
        Action<AnimatedValue<float>>? onFinish = null
    ) {
        return Animated(initialValue, SingleValueInterpolator.Instance, duration, curve, onFinish);
    }

    protected static SharedAnimatedValue<Vector2> Animated(
        Vector2 initialValue,
        AnimationDuration duration,
        AnimationCurve? curve = null,
        Action<AnimatedValue<Vector2>>? onFinish = null
    ) {
        return Animated(initialValue, Vector2ValueInterpolator.Instance, duration, curve, onFinish);
    }

    protected static SharedAnimatedValue<Vector3> Animated(
        Vector3 initialValue,
        AnimationDuration duration,
        AnimationCurve? curve = null,
        Action<AnimatedValue<Vector3>>? onFinish = null
    ) {
        return Animated(initialValue, Vector3ValueInterpolator.Instance, duration, curve, onFinish);
    }

    protected static SharedAnimatedValue<Color> Animated(
        Color initialValue,
        AnimationDuration duration,
        AnimationCurve? curve = null,
        Action<AnimatedValue<Color>>? onFinish = null
    ) {
        return Animated(initialValue, ColorValueInterpolator.Instance, duration, curve, onFinish);
    }

    protected static SharedAnimatedValue<TValue> Animated<TValue>(
        TValue initialValue,
        IValueInterpolator<TValue> interpolator,
        AnimationDuration duration,
        AnimationCurve? curve = null,
        Action<AnimatedValue<TValue>>? onFinish = null
    ) {
        return ValueUtils.Animated(initialValue, interpolator, duration, curve, onFinish);
    }

    #endregion

    #region Animate

    protected static ISharedAnimation Animation(Action onStart, params IEnumerable<ISharedAnimation> waitFor) {
        return AnimationUtils.Animation(onStart, waitFor);
    }

    #endregion

    #region Other

    /// <summary>
    /// Returns a canvas transform.
    /// </summary>
    protected RectTransform CanvasTransform => (RectTransform)Canvas!.transform;

    /// <summary>
    /// Creates a lazy value from a delegate.
    /// </summary>
    /// <param name="accessor">An accessor delegate.</param>
    /// <param name="cacheLazyValue">Should the lazy struct cache a value or not.
    /// When False, the delegate will be evaluated each time Value property is called.</param>
    protected static Lazy<T> Lazy<T>(Func<T> accessor, bool cacheLazyValue = true) {
        return new(accessor, cacheLazyValue);
    }

    /// <summary>
    /// Creates a scope with overrides for default values.
    /// </summary>
    protected static ContextScope Scope(params ContextPropChange[] changes) {
        return ContextScope.Declare(changes);
    }

    #endregion
}