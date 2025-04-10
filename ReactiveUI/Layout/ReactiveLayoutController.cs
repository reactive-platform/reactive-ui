using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Reactive {
    [PublicAPI]
    public abstract class ReactiveLayoutController : ILayoutController {
        #region LayoutController

        public abstract int ChildCount { get; }
        
        public event Action? LayoutControllerUpdatedEvent;

        protected void NotifyControllerUpdated() {
            LayoutControllerUpdatedEvent?.Invoke();
        }

        public abstract void InsertChild(ILayoutItem comp, int index);
        public abstract void RemoveChild(ILayoutItem comp);
        public abstract void RemoveAllChildren();
        public abstract void ApplyChildren();
        
        public abstract void Recalculate(ILayoutItem comp);
        public virtual void PrepareForRecalculation() { }

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