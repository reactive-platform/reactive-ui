﻿using System;
using UnityEngine;

namespace Reactive;

public partial class ReactiveComponentBase {
    protected static ObservableValue<TValue> Remember<TValue>(TValue initialValue) {
        return new ObservableValue<TValue>(initialValue);
    }

    protected AnimatedValue<Color> RememberAnimated(
        Color initialValue,
        AnimationDuration animationDuration,
        Action<AnimatedValue<Color>>? onFinish = null
    ) {
        return ValueUtils.RememberAnimatedColor(this, initialValue, animationDuration, onFinish);
    }

    protected AnimatedValue<Vector2> RememberAnimated(
        Vector2 initialValue,
        AnimationDuration animationDuration,
        Action<AnimatedValue<Vector2>>? onFinish = null
    ) {
        return ValueUtils.RememberAnimatedVector(this, initialValue, animationDuration, onFinish);
    }

    protected AnimatedValue<Vector3> RememberAnimated(
        Vector3 initialValue,
        AnimationDuration animationDuration,
        Action<AnimatedValue<Vector3>>? onFinish = null
    ) {
        return ValueUtils.RememberAnimatedVector(this, initialValue, animationDuration, onFinish);
    }

    protected AnimatedValue<float> RememberAnimated(
        float initialValue,
        AnimationDuration animationDuration,
        Action<AnimatedValue<float>>? onFinish = null
    ) {
        return ValueUtils.RememberAnimatedFloat(this, initialValue, animationDuration, onFinish);
    }

    protected AnimatedValue<TValue> RememberAnimated<TValue>(
        TValue initialValue,
        IValueInterpolator<TValue> interpolator,
        AnimationDuration duration,
        Action<AnimatedValue<TValue>>? onFinish = null
    ) {
        return ValueUtils.RememberAnimated(this, initialValue, interpolator, duration, onFinish);
    }
}