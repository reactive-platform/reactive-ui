using System;
using JetBrains.Annotations;

namespace Reactive;

/// <summary>
/// A non-generic abstraction over ContextProp to bypass invariance.
/// </summary>
[PublicAPI]
public interface IContextProp {
    Func<object>? Factory { get; set; }
}