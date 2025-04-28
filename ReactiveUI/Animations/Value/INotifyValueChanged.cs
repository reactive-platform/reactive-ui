using System;
using JetBrains.Annotations;

namespace Reactive {
    /// <summary>
    /// Represents an object holding a value that can be updated.
    /// </summary>
    /// <typeparam name="T">Value type.</typeparam>
    [PublicAPI]
    public interface INotifyValueChanged<out T> {
        T Value { get; }

        event Action<T>? ValueChangedEvent;
    }
}