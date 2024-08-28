using JetBrains.Annotations;

namespace Reactive {
    [PublicAPI]
    public interface IReactiveModuleBinder {
        void BindModule(IReactiveModule module);
        void UnbindModule(IReactiveModule module);
    }
}