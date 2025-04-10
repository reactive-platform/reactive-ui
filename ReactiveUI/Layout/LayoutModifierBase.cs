using System;
using JetBrains.Annotations;

namespace Reactive {
    [PublicAPI]
    public abstract class LayoutModifierBase<T> : ILayoutModifier where T : ILayoutModifier, new() {
        #region Modifier

        protected bool SuppressRefresh { get; set; }

        public event Action? ModifierUpdatedEvent;

        protected void NotifyModifierUpdated() {
            if (SuppressRefresh) {
                return;
            }
            ModifierUpdatedEvent?.Invoke();
        }

        public virtual void ExposeLayoutItem(ILayoutItem item) { }
        
        public virtual void CopyFromSimilar(T similar) { }

        public virtual void CopyFrom(ILayoutModifier mod) {
            if (mod is T similar) {
                CopyFromSimilar(similar);
            }
        }

        public virtual ILayoutModifier CreateCopy() {
            var n = new T();
            n.CopyFrom(this);
            return n;
        }

        #endregion

        #region Context

        public virtual Type? ContextType => null;

        public virtual object CreateContext() {
            throw new NotSupportedException();
        }

        public virtual void ProvideContext(object? context) { }

        #endregion
    }
}