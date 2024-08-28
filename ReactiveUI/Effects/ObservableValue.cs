using System;
using JetBrains.Annotations;

namespace Reactive {
    [PublicAPI]
    public class ObservableValue<T> : INotifyValueChanged<T> {
        public ObservableValue(T initialValue) {
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
    }
}