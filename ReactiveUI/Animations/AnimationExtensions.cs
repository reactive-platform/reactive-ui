using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Reactive {
    [PublicAPI]
    public static class AnimationExtensions {
        public static T Animate<T, TValue>(
            this T comp,
            INotifyValueChanged<TValue> value,
            Expression<Func<T, TValue>> expression,
            bool applyImmediately = true
        ) where T : IReactiveComponent {
            var setter = expression.GeneratePropertySetter();

            return Animate(comp, value, setter, applyImmediately);
        }

        public static T Animate<T, TValue>(
            this T comp,
            INotifyValueChanged<TValue> value,
            Action onEffect,
            bool applyImmediately = true
        ) where T : IReactiveComponent {
            return Animate(comp, value, (_, _) => onEffect(), applyImmediately);
        }

        public static T Animate<T, TValue>(
            this T comp,
            INotifyValueChanged<TValue> value,
            Action<T, TValue> onEffect,
            bool applyImmediately = true
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
    }
}