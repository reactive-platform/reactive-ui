using System;
using JetBrains.Annotations;

namespace Reactive {
    /// <summary>
    /// Represents an object that can produce and consume shared contexts.
    /// </summary>
    [PublicAPI]
    public interface IContextMember {
        Type? ContextType { get; }
        
        object CreateContext();
        void ProvideContext(object? context);
    }
}