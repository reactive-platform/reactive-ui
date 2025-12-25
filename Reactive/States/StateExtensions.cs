using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Reactive {
    [PublicAPI]
    public static class StateExtensions {
        #region Queries

        /// <param name="state">A state to wrap.</param>
        /// <typeparam name="T">An initial type of the state.</typeparam>
        extension<T>(IState<T> state) {
            /// <summary>
            /// Maps a state to another type.
            /// </summary>
            /// <param name="predicate">A mapping predicate.</param>
            /// <typeparam name="TMap">A type of the mapped state.</typeparam>
            /// <returns>A wrapper over the original state instance.</returns>
            public MappedState<T, TMap> Map<TMap>(Func<T, TMap> predicate) {
                return new(state, predicate);
            }

            /// <summary>
            /// Filters state updates by some condition.
            /// </summary>
            /// <param name="predicate">A filtering predicate.</param>
            /// <returns>A wrapper over the original state instance.</returns>
            public BranchedState<T> Where(Func<T, bool> predicate) {
                return new(state, predicate);
            }
        }

        #endregion

        #region Animate
        
        // Note: methods use duplicated logic rather than wrapping
        // as each wrap costs a heap allocation
        extension<T>(T comp) where T : IReactiveComponent {
            /// <summary>
            /// This method uses expression parsing to generate callbacks in runtime.
            /// Runtime generation is very expensive and is already replaced by
            /// much faster and convenient property extensions, so this method is considered obsolete now.
            /// </summary>
            [Obsolete("Use bindings generator instead")]
            public T Animate<TValue>(IState<TValue> value, Expression<Func<T, TValue>> expression, bool applyImmediately = false) {
                var setter = expression.GeneratePropertySetter();

                return On(comp, value, setter, applyImmediately);
            }

            /// <summary>
            /// Binds a callback to some state.
            /// </summary>
            /// <param name="value">A state to bind to.</param>
            /// <param name="onEffect">A callback to bind.</param>
            /// <param name="applyImmediately">Whether the callback should be invoked immediately.</param>
            public T On<TValue>(IState<TValue> value, Action<TValue> onEffect, bool applyImmediately = false) {
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

                    onEffect(val);
                }

                value.ValueChangedEvent += Closure;

                if (applyImmediately) {
                    Closure(value.Value);
                }

                return comp;
            }
            
            /// <summary>
            /// Binds a callback to some state.
            /// </summary>
            /// <param name="value">A state to bind to.</param>
            /// <param name="onEffect">A callback to bind.</param>
            /// <param name="applyImmediately">Whether the callback should be invoked immediately.</param>
            public T On<TValue>(IState<TValue> value, Action<T, TValue> onEffect, bool applyImmediately = false) {
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
        }

        #endregion
    }
}