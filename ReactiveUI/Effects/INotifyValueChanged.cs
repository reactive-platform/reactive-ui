using System;
using JetBrains.Annotations;

namespace Reactive {
    [PublicAPI]
    public interface INotifyValueChanged<out T> {
        T Value { get; }

        event Action<T>? ValueChangedEvent;
    }
}