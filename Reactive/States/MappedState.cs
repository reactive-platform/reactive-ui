using System;
using JetBrains.Annotations;

namespace Reactive;

/// <summary>
/// A wrapper class over <see cref="IState"/> that enables
/// an ability to map a state value.
/// </summary>
[PublicAPI]
public class MappedState<T, TNew> : IState<TNew>, IDisposable {
    /// <summary>
    /// Represents a state value. Evaluated on each call.
    /// </summary>
    public TNew Value => _predicate(_state.Value);
    
    public event Action<TNew>? ValueChangedEvent;

    private readonly IState<T> _state;
    private readonly Func<T, TNew> _predicate;

    public MappedState(IState<T> state, Func<T, TNew> predicate) {
        _state = state;
        _predicate = predicate;
        state.ValueChangedEvent += HandleValueChanged;
    }

    private void HandleValueChanged(T value) {
        ValueChangedEvent?.Invoke(_predicate(value));
    }

    public void Dispose() {
        _state.ValueChangedEvent -= HandleValueChanged;
    }
}