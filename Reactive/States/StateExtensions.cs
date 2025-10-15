using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Reactive {
    [PublicAPI]
    public static class StateExtensions {
        #region Queries

        /// <summary>
        /// Maps a state to another type.
        /// </summary>
        /// <param name="state">A state to wrap.</param>
        /// <param name="predicate">A mapping predicate.</param>
        /// <typeparam name="T">An initial type of the state.</typeparam>
        /// <typeparam name="TMap">A type of the mapped state.</typeparam>
        /// <returns>A wrapper over the original state instance.</returns>
        public static MappedState<T, TMap> Map<T, TMap>(this IState<T> state, Func<T, TMap> predicate) {
            return new(state, predicate);
        }

        /// <summary>
        /// Filters state updates by some condition.
        /// </summary>
        /// <param name="state">A state to wrap.</param>
        /// <param name="predicate">A filtering predicate.</param>
        /// <typeparam name="T">A type of the state.</typeparam>
        /// <returns>A wrapper over the original state instance.</returns>
        public static BranchedState<T> Where<T>(this IState<T> state, Func<T, bool> predicate) {
            return new(state, predicate);
        }

        #endregion

        #region Animate

        public static T Animate<T, TValue>(
            this T comp,
            IState<TValue> value,
            Expression<Func<T, TValue>> expression,
            bool applyImmediately = false
        ) where T : IReactiveComponent {
            var setter = expression.GeneratePropertySetter();

            return Animate(comp, value, setter, applyImmediately);
        }

        public static T Animate<T, TValue>(
            this T comp,
            IState<TValue> value,
            Action onEffect,
            bool applyImmediately = false
        ) where T : IReactiveComponent {
            return Animate(comp, value, (_, _) => onEffect(), applyImmediately);
        }

        public static T Animate<T, TValue>(
            this T comp,
            IState<TValue> value,
            Action<T, TValue> onEffect,
            bool applyImmediately = false
        ) where T : IReactiveComponent {
            void Closure(TValue val) {
                // Return if component is not valid yet
                if (!comp.IsInitialized) {
                    return;
                }

                // Unsubscribe if component is not valid anymore
                if (comp.IsDestroyed) {
                    value.ValueChangedEvent -= Closure;
                    return;
                }

                onEffect(comp, val);
            }

            value.ValueChangedEvent += Closure;

            if (applyImmediately) {
                Closure(value.Value);
            }

            return comp;
        }

        #endregion
    }
}