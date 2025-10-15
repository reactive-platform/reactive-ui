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

    protected static State<TValue> Remember<TValue>(TValue initialValue) {
        return new State<TValue>(initialValue);
    }

    protected AnimatedState<Color> RememberAnimated(
        Color initialValue,
        AnimationDuration animationDuration,
        AnimationCurve? curve = null,
        Action<Color>? onFinish = null
    ) {
        return StateUtils.RememberAnimatedColor(this, initialValue, animationDuration, curve, onFinish);
    }

    protected AnimatedState<Vector2> RememberAnimated(
        Vector2 initialValue,
        AnimationDuration animationDuration,
        AnimationCurve? curve = null,
        Action<Vector2>? onFinish = null
    ) {
        return StateUtils.RememberAnimatedVector(this, initialValue, animationDuration, curve, onFinish);
    }

    protected AnimatedState<Vector3> RememberAnimated(
        Vector3 initialValue,
        AnimationDuration animationDuration,
        AnimationCurve? curve = null,
        Action<Vector3>? onFinish = null
    ) {
        return StateUtils.RememberAnimatedVector(this, initialValue, animationDuration, curve, onFinish);
    }

    protected AnimatedState<float> RememberAnimated(
        float initialValue,
        AnimationDuration animationDuration,
        AnimationCurve? curve = null,
        Action<float>? onFinish = null
    ) {
        return StateUtils.RememberAnimatedFloat(this, initialValue, animationDuration, curve, onFinish);
    }

    protected AnimatedState<TValue> RememberAnimated<TValue>(
        TValue initialValue,
        IValueInterpolator<TValue> interpolator,
        AnimationDuration duration,
        AnimationCurve? curve = null,
        Action<TValue>? onFinish = null
    ) {
        return StateUtils.RememberAnimated(this, initialValue, interpolator, duration, curve, onFinish);
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

    #endregion
}