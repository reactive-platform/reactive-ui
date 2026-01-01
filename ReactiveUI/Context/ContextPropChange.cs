using System;
using JetBrains.Annotations;

namespace Reactive;

/// <summary>
/// A non-generic store for prop and factory that's used to avoid boxing when passing scope params.
/// </summary>
[PublicAPI]
public struct ContextPropChange {
    private ContextPropChange(IContextProp prop, Func<object>? newFactory) {
        Prop = prop;
        NewFactory = newFactory;
        InitialFactory = prop.Factory;
    }

    /// <summary>
    /// A property to manage.
    /// </summary>
    public readonly IContextProp Prop;
    
    /// <summary>
    /// A factory to set to the property.
    /// </summary>
    public readonly Func<object>? NewFactory;
    
    /// <summary>
    /// A factory that was assigned to the property initially.
    /// </summary>
    public readonly Func<object>? InitialFactory;

    public void SetNewFactory() {
        Prop.Factory = NewFactory;
    }

    public void SetInitialFactory() {
        Prop.Factory = InitialFactory;
    }

    /// <summary>
    /// Creates a single-property scope from this structure.
    /// Use only when you want to override just this property and nothing else.
    /// </summary>
    public ContextScope ToScope() {
        return new ContextScope([this]);
    }

    public static ContextPropChange Create<T>(ContextProp<T> prop, Func<T>? factory) where T : class {
        return new(prop, factory);
    }
}