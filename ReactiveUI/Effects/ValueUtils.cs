using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Reactive {
    [PublicAPI]
    public static class ValueUtils {
        #region Remember

        

        #endregion
        
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
                Curve = curve ?? AnimationCurve.Exponential
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
        
        public static IObjectAnimator<T> Animate<T>(
            IReactiveModuleBinder binder,
            Action<T, float> callback,
            AnimationDuration duration,
            AnimationCurve? curve = null
        ) {
            var animator = new ObjectAnimator<T>(callback) {
                Duration = duration,
                Curve = curve ?? AnimationCurve.Exponential
            };
            binder.BindModule(animator);
            return animator;
        }
    }
}