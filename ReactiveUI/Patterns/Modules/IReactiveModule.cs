using JetBrains.Annotations;

namespace Reactive {
    [PublicAPI]
    public interface IReactiveModule {
        void OnUpdate();
    }
}