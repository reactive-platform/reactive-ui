using System;
using JetBrains.Annotations;

namespace Reactive;

/// <summary>
/// Defines a context scope bound to the function scope.
/// </summary>
[PublicAPI]
public ref struct ContextScope : IDisposable {
    private readonly ContextPropChange[] _changes;
    private bool _initialized;

    public ContextScope(ContextPropChange[] changes) {
        foreach (var prop in changes) {
            prop.SetNewFactory();
        }
        
        _changes = changes;
        _initialized = true;
    }

    public void Dispose() {
        _initialized = false;
        
        foreach (var prop in _changes) {
            prop.SetInitialFactory();
        }
    }

    public static ContextScope Declare(params ContextPropChange[] changes) {
        return new(changes);
    }
}