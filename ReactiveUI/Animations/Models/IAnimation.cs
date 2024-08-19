using JetBrains.Annotations;

namespace Reactive {
    [PublicAPI]
    public interface IAnimation {
        bool IsFinished { get; }
        
        string? PropertyName { get; }
        object? Target { get; }

        void Evaluate(float delta);
        void Reset();
    }
}