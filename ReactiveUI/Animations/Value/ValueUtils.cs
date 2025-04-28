using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Reactive {
    [PublicAPI]
    public static class ValueUtils {
        #region Owned

        public static ObservableValue<T> Remember<T>(T initialValue) {
            return new ObservableValue<T>(initialValue);
        }

        public static AnimatedValue<T> RememberAnimated<T>(
            IReactiveModuleBinder binder,
            T initialValue,
            IValueInterpolator<T> interpolator,
            AnimationDuration duration,
            AnimationCurve? curve = null,
            Action<AnimatedValue<T>>? onFinish = null
        ) {
            var value = new AnimatedValue<T>(initialValue, interpolator) {
                Duration = duration,
                OnFinish = onFinish,
                Curve = curve ?? AnimationCurve.Linear
            };
            binder.BindModule(value);
            return value;
        }

        public static AnimatedValue<Color> RememberAnimatedColor(
            IReactiveModuleBinder binder,
            Color initialValue,
            AnimationDuration duration,
            AnimationCurve? curve = null,
            Action<AnimatedValue<Color>>? onFinish = null
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

        public static AnimatedValue<Vector2> RememberAnimatedVector(
            IReactiveModuleBinder binder,
            Vector2 initialValue,
            AnimationDuration duration,
            AnimationCurve? curve = null,
            Action<AnimatedValue<Vector2>>? onFinish = null
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

        public static AnimatedValue<Vector3> RememberAnimatedVector(
            IReactiveModuleBinder binder,
            Vector3 initialValue,
            AnimationDuration duration,
            AnimationCurve? curve = null,
            Action<AnimatedValue<Vector3>>? onFinish = null
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

        public static AnimatedValue<float> RememberAnimatedFloat(
            IReactiveModuleBinder binder,
            float initialValue,
            AnimationDuration duration,
            AnimationCurve? curve = null,
            Action<AnimatedValue<float>>? onFinish = null
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

        #region Unowned

        public static SharedAnimatedValue<T> Animated<T>(
            T initialValue,
            IValueInterpolator<T> interpolator,
            AnimationDuration duration,
            AnimationCurve? curve = null,
            Action<AnimatedValue<T>>? onFinish = null
        ) {
            return new SharedAnimatedValue<T>(initialValue, interpolator) {
                Duration = duration,
                OnFinish = onFinish,
                Curve = curve ?? AnimationCurve.Linear
            };
        }

        public static SharedAnimatedValue<Color> AnimatedColor(
            Color initialValue,
            AnimationDuration duration,
            AnimationCurve? curve = null,
            Action<AnimatedValue<Color>>? onFinish = null
        ) {
            return Animated(
                initialValue,
                ColorValueInterpolator.Instance,
                duration,
                curve,
                onFinish
            );
        }

        public static SharedAnimatedValue<Vector2> AnimatedVector2(
            Vector2 initialValue,
            AnimationDuration duration,
            AnimationCurve? curve = null,
            Action<AnimatedValue<Vector2>>? onFinish = null
        ) {
            return Animated(
                initialValue,
                Vector2ValueInterpolator.Instance,
                duration,
                curve,
                onFinish
            );
        }

        public static SharedAnimatedValue<Vector3> AnimatedVector3(
            Vector3 initialValue,
            AnimationDuration duration,
            AnimationCurve? curve = null,
            Action<AnimatedValue<Vector3>>? onFinish = null
        ) {
            return Animated(
                initialValue,
                Vector3ValueInterpolator.Instance,
                duration,
                curve,
                onFinish
            );
        }

        public static SharedAnimatedValue<float> AnimatedFloat(
            float initialValue,
            AnimationDuration duration,
            AnimationCurve? curve = null,
            Action<AnimatedValue<float>>? onFinish = null
        ) {
            return Animated(
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