using System;
using JetBrains.Annotations;

namespace Reactive;

/// <summary>
/// A struct that provides lazy access to a value.
/// </summary>
/// <typeparam name="T">A type of lazy value.</typeparam>
[PublicAPI]
public struct Lazy<T> {
    /// <summary>
    /// Creates a lazy value from a delegate.
    /// </summary>
    /// <param name="accessor">An accessor delegate.</param>
    /// <param name="cacheLazyValue">Should the lazy struct cache a value or not.
    /// When False, the delegate will be evaluated each time Value property is called.</param>
    public Lazy(Func<T> accessor, bool cacheLazyValue = true) {
        _accessor = accessor;
        _cacheValue = cacheLazyValue;
    }

    public Lazy(T value) {
        _value = value;
        _cacheValue = true;
    }

    public T Value {
        get {
            if (_value == null || !_cacheValue) {
                if (_accessor == null) {
                    throw new InvalidOperationException("Accessor cannot be null");
                }
                
                _value = _accessor();
            }

            return _value;
        }
    }

    private Func<T>? _accessor;
    private bool _cacheValue;
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