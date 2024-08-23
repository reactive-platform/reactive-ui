using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Reactive {
    [PublicAPI]
    public static class AnimationExtensions {
        #region Transition Extensions

        public static T WithTransition<T, TValue>(
            this T target,
            ComponentState state,
            Expression<Func<T, TValue>> expression,
            TValue to,
            AnimationDuration duration,
            AnimationCurve curve,
            ComponentState reverseState,
            TValue reverseValue
        ) where T : IStateAnimationHost {
            WithTransition(
                target,
                out var transition,
                state,
                expression,
                to,
                duration,
                curve
            );
            transition = transition.CreateCopy(reverseValue);
            target.AddStateAnimation(reverseState, transition);
            return target;
        }

        public static T WithTransition<T, TValue>(
            this T target,
            ComponentState state,
            Expression<Func<T, TValue>> expression,
            TValue to,
            AnimationDuration duration,
            AnimationCurve? curve = null
        ) where T : IStateAnimationHost {
            return WithTransition(
                target,
                out _,
                state,
                expression,
                to,
                duration,
                curve
            );
        }

        public static T WithTransition<T, TValue>(
            this T target,
            out Transition<T, TValue> transition,
            ComponentState state,
            Expression<Func<T, TValue>> expression,
            TValue to,
            AnimationDuration duration,
            AnimationCurve? curve = null
        ) where T : IStateAnimationHost {
            var interpolator = ValueInterpolator.GetInterpolator<TValue>();
            transition = new Transition<T, TValue>(target, expression, to, interpolator) {
                Duration = duration,
                Curve = curve.GetValueOrDefault(AnimationBasicCurve.EaseInOut)
            };
            target.AddStateTransition(state, transition);
            return target;
        }

        #endregion

        #region Animation Extensions

        public static T WithStateAnimation<T, TValue>(
            this T target,
            ComponentState state,
            Expression<Func<T, TValue>> expression,
            TValue from,
            TValue to,
            AnimationDuration duration,
            AnimationCurve? curve = null,
            AnimationRepeats? repeats = null,
            AnimationDirection direction = AnimationDirection.Normal
        ) where T : IStateAnimationHost {
            var animation = target.CreateValueAnimation(
                expression,
                from,
                to,
                ValueInterpolator.GetInterpolator<TValue>(),
                duration,
                curve.GetValueOrDefault(AnimationBasicCurve.EaseInOut),
                repeats.GetValueOrDefault(1),
                direction
            );
            target.AddStateAnimation(state, animation);
            return target;
        }

        public static T WithAnimation<T, TValue>(
            this T target,
            Expression<Func<T, TValue>> expression,
            TValue from,
            TValue to,
            AnimationDuration duration,
            AnimationCurve? curve = null,
            AnimationRepeats? repeats = null,
            AnimationDirection direction = AnimationDirection.Normal
        ) where T : IAnimationHost {
            var animation = target.CreateValueAnimation(
                expression,
                from,
                to,
                ValueInterpolator.GetInterpolator<TValue>(),
                duration,
                curve.GetValueOrDefault(AnimationBasicCurve.EaseInOut),
                repeats.GetValueOrDefault(1),
                direction
            );
            target.AddAnimation(animation);
            return target;
        }

        #endregion

        #region Factory

        public static Animation<T> CreateValueAnimation<T, TValue>(
            this T target,
            Expression<Func<T, TValue>> expression,
            TValue from,
            TValue to,
            ValueInterpolator<TValue> interpolator,
            AnimationDuration duration,
            AnimationCurve curve,
            AnimationRepeats repeats,
            AnimationDirection direction = AnimationDirection.Normal
        ) {
            var propertyName = expression.GetPropertyNameOrThrow();
            var timeline = CreateValueTimeline(expression, from, to, interpolator);
            return CreateValueAnimation(target, timeline, duration, curve, repeats, direction, propertyName);
        }

        public static Animation<T> CreateValueAnimation<T>(
            this T target,
            IAnimationTimeline<T> timeline,
            AnimationDuration duration,
            AnimationCurve curve,
            AnimationRepeats repeats,
            AnimationDirection direction = AnimationDirection.Normal,
            string? propertyName = null
        ) {
            return new Animation<T>(target, timeline, propertyName) {
                Repeats = repeats,
                Duration = duration,
                Curve = curve,
                Direction = direction
            };
        }

        public static ValueTimeline<T, TValue> CreateValueTimeline<T, TValue>(
            Expression<Func<T, TValue>> expression,
            TValue from, TValue to,
            ValueInterpolator<TValue> interpolator
        ) {
            var applicator = expression.GeneratePropertySetter();
            return new ValueTimeline<T, TValue>(applicator, interpolator) {
                FromValue = from,
                ToValue = to
            };
        }

        #endregion
    }
}