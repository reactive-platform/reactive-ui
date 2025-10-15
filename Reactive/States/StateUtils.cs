using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Reactive {
    [PublicAPI]
    public static class StateUtils {
        #region Owned

        public static State<T> Remember<T>(T initialValue) {
            return new State<T>(initialValue);
        }

        public static AnimatedState<T> RememberAnimated<T>(
            IReactiveModuleBinder binder,
            T initialValue,
            IValueInterpolator<T> interpolator,
            AnimationDuration duration,
            AnimationCurve? curve = null,
            Action<T>? onFinish = null
        ) {
            var value = new AnimatedState<T>(initialValue, interpolator) {
                Duration = duration,
                OnFinish = onFinish,
                Curve = curve ?? AnimationCurve.Linear
            };
            binder.BindModule(value);
            return value;
        }

        public static AnimatedState<Color> RememberAnimatedColor(
            IReactiveModuleBinder binder,
            Color initialValue,
            AnimationDuration duration,
            AnimationCurve? curve = null,
            Action<Color>? onFinish = null
        ) {
            return RememberAnimated(
                binder,
                initialValue,
                ColorValueInterpolator.Instance,
                duration,
                curve,
                onFinish
            );
        }

        public static AnimatedState<Vector2> RememberAnimatedVector(
            IReactiveModuleBinder binder,
            Vector2 initialValue,
            AnimationDuration duration,
            AnimationCurve? curve = null,
            Action<Vector2>? onFinish = null
        ) {
            return RememberAnimated(
                binder,
                initialValue,
                Vector2ValueInterpolator.Instance,
                duration,
                curve,
                onFinish
            );
        }

        public static AnimatedState<Vector3> RememberAnimatedVector(
            IReactiveModuleBinder binder,
            Vector3 initialValue,
            AnimationDuration duration,
            AnimationCurve? curve = null,
            Action<Vector3>? onFinish = null
        ) {
            return RememberAnimated(
                binder,
                initialValue,
                Vector3ValueInterpolator.Instance,
                duration,
                curve,
                onFinish
            );
        }

        public static AnimatedState<float> RememberAnimatedFloat(
            IReactiveModuleBinder binder,
            float initialValue,
            AnimationDuration duration,
            AnimationCurve? curve = null,
            Action<float>? onFinish = null
        ) {
            return RememberAnimated(
                binder,
                initialValue,
                SingleValueInterpolator.Instance,
                duration,
                curve,
                onFinish
            );
        }

        #endregion
    }
}