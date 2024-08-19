using System;

namespace Reactive {
    public interface IContextMember {
        Type? ContextType { get; }
        
        object CreateContext();
        void ProvideContext(object? context);
    }
}