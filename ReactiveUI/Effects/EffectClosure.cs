using System;

namespace Reactive {
    public class EffectClosure<T, TValue> : IEffect<TValue> {
        public EffectClosure(T component, Action<T, TValue> onEffect) {
            _component = component;
            _onEffect = onEffect;
        }

        private readonly T _component;
        private readonly Action<T, TValue> _onEffect;

        public void Invoke(TValue value) {
            _onEffect(_component, value);
        }
    }
}