using System;
using JetBrains.Annotations;

namespace Reactive {
    /// <summary>
    /// Represents a reactive state.
    /// </summary>
    /// <typeparam name="T">A type of the state.</typeparam>
    [PublicAPI]
    public interface IState<out T> {
        T Value { get; }

        /// <summary>
        /// Fired whenever the state is updated.
        /// </summary>
        event Action<T>? ValueChangedEvent;
    }
}