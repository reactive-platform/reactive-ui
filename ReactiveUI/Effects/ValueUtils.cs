using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Reactive {
    [PublicAPI]
    public static class ValueUtils {
        public static ObservableValue<T> Remember<T>(T initialValue) {
            return new ObservableValue<T>(initialValue);
        }

        public static AnimatedValue<T> RememberAnimated<T>(
            IReactiveModuleBinder binder,
            T initialValue,
            IValueInterpolator<T> interpolator,
            AnimationDuration duration,
            Action<AnimatedValue<T>>? onFinish = null
        ) {
            var value = new AnimatedValue<T>(initialValue, interpolator) {
                Duration = duration,
                OnFinish = onFinish
            };
            binder.BindModule(value);
            return value;
        }

        public static AnimatedValue<Color> RememberAnimatedColor(
            IReactiveModuleBinder binder,
            Color initialValue,
            AnimationDuration duration,
            Action<AnimatedValue<Color>>? onFinish = null
        ) {
            return RememberAnimated(
                binder,
                initialValue,
                ColorValueInterpolator.Instance,
                duration,
                onFinish
            );
        }

        public static AnimatedValue<Vector2> RememberAnimatedVector(
            IReactiveModuleBinder binder,
            Vector2 initialValue,
            AnimationDuration duration,
            Action<AnimatedValue<Vector2>>? onFinish = null
        ) {
            return RememberAnimated(
                binder,
                initialValue,
                Vector2ValueInterpolator.Instance,
                duration,
                onFinish
            );
        }

        public static AnimatedValue<Vector3> RememberAnimatedVector(
            IReactiveModuleBinder binder,
            Vector3 initialValue,
            AnimationDuration duration,
            Action<AnimatedValue<Vector3>>? onFinish = null
        ) {
            return RememberAnimated(
                binder,
                initialValue,
                Vector3ValueInterpolator.Instance,
                duration,
                onFinish
            );
        }

        public static AnimatedValue<float> RememberAnimatedFloat(
            IReactiveModuleBinder binder,
            float initialValue,
            AnimationDuration duration,
            Action<AnimatedValue<float>>? onFinish = null
        ) {
            return RememberAnimated(
                binder,
                initialValue,
                SingleValueInterpolator.Instance,
                duration,
                onFinish
            );
        }
    }
}