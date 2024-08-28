using System;
using JetBrains.Annotations;

namespace Reactive {
    [PublicAPI]
    public interface INotifyValueChanged<out T> : INotifyValueChanged {
        T Value { get; }

        event Action<T>? ValueChangedEvent;
    }

    [PublicAPI]
    public interface INotifyValueChanged {
        void ClearBindings();
    }
}