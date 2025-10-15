using System;
using JetBrains.Annotations;

namespace Reactive {
    [PublicAPI]
    public class State<T> : IState<T> {
        public State(T initialValue) {
            _value = initialValue;
        }

        public T Value {
            get => _value;
            set {
                _value = value;
                ValueChangedEvent?.Invoke(value);
            }
        }

        public event Action<T>? ValueChangedEvent;

        private T _value;

        public void ClearBindings() {
            ValueChangedEvent = null;
        }

        public static implicit operator T(State<T> value) {
            return value._value;
        }
    }
}