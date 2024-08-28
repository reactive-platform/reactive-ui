using System;
using JetBrains.Annotations;

namespace Reactive {
    [PublicAPI]
    public static class EffectExtensions {
        public static T WithEffect<T, TValue>(
            this T comp,
            INotifyValueChanged<TValue> value,
            Action<T, TValue> onEffect
        ) where T : IEffectBinder {
            var effect = new EffectClosure<T, TValue>(comp, onEffect);
            effect.Invoke(value.Value);
            comp.BindEffect(value, effect);
            return comp;
        }
    }
}