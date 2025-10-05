using System;
using JetBrains.Annotations;

namespace Reactive;

/// <summary>
/// A wrapper class over <see cref="IState"/> that enables
/// an ability to set a condition over state updates.
/// </summary>
[PublicAPI]
public class BranchedState<T> : IState<T>, IDisposable {
    /// <summary>
    /// Represents a state value. Keep in mind that it returns the last value
    /// that met branching conditions, so it's not guaranteed that the value is equal to the original one.
    /// However, this value will always be equal to the original one before
    /// the first update, no matter it matches the condition or not.
    /// </summary>
    public T Value { get; private set; }

    public event Action<T>? ValueChangedEvent;

    private readonly IState<T> _state;
    private readonly Func<T, bool> _predicate;

    public BranchedState(IState<T> state, Func<T, bool> predicate) {
        _state = state;
        _predicate = predicate;
        
        Value = state.Value;
        state.ValueChangedEvent += HandleValueChanged;
    }

    private void HandleValueChanged(T value) {
        if (_predicate(value)) {
            Value = value;
            ValueChangedEvent?.Invoke(value);
        }
    }

    public void Dispose() {
        _state.ValueChangedEvent -= HandleValueChanged;
    }
}