using JetBrains.Annotations;

namespace Reactive {
    [PublicAPI]
    public interface IEffect<in T> {
        void Invoke(T value);
    }
}