using JetBrains.Annotations;

namespace Reactive {
    [PublicAPI]
    public interface IEffectBinder {
        void BindEffect<T>(INotifyValueChanged<T> value, IEffect<T> effect);
        void UnbindEffect<T>(INotifyValueChanged<T> value, IEffect<T> effect);
    }
}