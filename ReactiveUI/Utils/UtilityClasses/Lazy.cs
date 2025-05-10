using System;
using JetBrains.Annotations;

namespace Reactive;

/// <summary>
/// A struct that provides lazy access to a value.
/// </summary>
/// <typeparam name="T">A type of lazy value.</typeparam>
[PublicAPI]
public struct Lazy<T> {
    public Lazy(Func<T> accessor) {
        _accessor = accessor;
    }

    public Lazy(T value) {
        _value = value;
    }

    public T Value {
        get {
            if (_value == null) {
                if (_accessor == null) {
                    throw new InvalidOperationException("Accessor cannot be null");
                }
                
                _value = _accessor();
            }

            return _value;
        }
    }

    private Func<T>? _accessor;
    private T? _value;
    
    public static implicit operator Lazy<T>(Func<T> accessor) {
        return new(accessor);
    }
    
    public static implicit operator Lazy<T>(T value) {
        return new(value);
    }

    public static implicit operator T(Lazy<T> lazy) {
        return lazy.Value;
    }
}