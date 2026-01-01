using System;
using JetBrains.Annotations;

namespace Reactive;

/// <summary>
/// A property metadata used alongside with <see cref="ComponentDefaults"/>.
/// </summary>
/// <typeparam name="T">A type of the property.</typeparam>
[PublicAPI]
public class ContextProp<T> : IContextProp where T : class {
    public Func<T>? Factory { get; set; }

    Func<object>? IContextProp.Factory {
        get => Factory;
        set => Factory = (Func<T>?)value;
    }

    public bool TryCreate(out T? value) {
        if (Factory == null) {
            value = null;
            return false;
        }

        value = Factory();
        return true;
    }

    /// <summary>
    /// Creates an override structure that can be used to apply and rollback changes.
    /// </summary>
    /// <param name="factory">A factory to use for value production.</param>
    public ContextPropChange Override(Func<T>? factory) {
        return ContextPropChange.Create(this, factory);
    }
    
    /// <summary>
    /// Assigns an optional value if it's not set.
    /// </summary>
    /// <param name="optional">A value to assign.</param>
    /// <returns>True if the assignment has happened, False if it did not.</returns>
    public bool AssignOptional(ref Optional<T?> optional)  {
        if (!optional.HasValue && TryCreate(out var value)) {
            optional = Optional<T?>.Some(value);
            return true;
        }

        return false;
    }
}