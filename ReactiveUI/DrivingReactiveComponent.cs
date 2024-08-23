using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Reactive {
    [PublicAPI]
    public abstract class DrivingReactiveComponent : DrivingReactiveComponentBase {
        /// <summary>
        /// Represents the children of the component.
        /// </summary>
        public new ICollection<ILayoutItem> Children => base.Children;
    }

    [PublicAPI]
    public abstract class DrivingReactiveComponentBase : ReactiveComponentBase, ILayoutDriver {
        #region Layout Controller

        public ILayoutController? LayoutController {
            get => _layoutController;
            set {
                if (_layoutController != null) {
                    ReleaseContextMember(_layoutController);
                    _layoutController.LayoutControllerUpdatedEvent -= ScheduleLayoutRecalculation;
                }
                _layoutController = value;
                if (_layoutController != null) {
                    InsertContextMember(_layoutController);
                    _layoutController.LayoutControllerUpdatedEvent += ScheduleLayoutRecalculation;
                }
                RecalculateLayoutWithChildren();
            }
        }

        private ILayoutController? _layoutController;
        private bool _beingRecalculated;
        private bool _recalculationScheduled;

        private void RecalculateLayoutWithChildren() {
            _layoutController?.ReloadChildren(_children);
            ScheduleLayoutRecalculation();
        }

        private void RecalculateLayoutInternal(bool root) {
            if (_layoutController == null || Children.Count == 0) return;
            _layoutController.ReloadDimensions(ContentTransform.rect);
            _layoutController.Recalculate(root);
            if (root) {
                _layoutController.ApplySelf(this);
            }
            _layoutController.ApplyChildren();
        }

        public void RecalculateLayoutTree() {
            _beingRecalculated = true;
            //items without modifiers are not supposed to be controlled
            if (LayoutModifier != null && LayoutDriver?.LayoutController != null) {
                LayoutDriver!.RecalculateLayoutTree();
                _beingRecalculated = false;
                return;
            }
            RecalculateLayoutInternal(true);
            _beingRecalculated = false;
        }

        public void RecalculateLayout() {
            RecalculateLayoutInternal(false);
        }

        private void ScheduleLayoutRecalculation() {
            _recalculationScheduled = true;
        }

        #endregion

        #region Children

        /// <summary>
        /// Represents the children of the component.
        /// </summary>
        protected ICollection<ILayoutItem> Children => _children;

        IEnumerable<ILayoutItem> ILayoutDriver.Children => Children;

        private ObservableSet<ILayoutItem> _children = null!;

        void ILayoutDriver.AppendChild(ILayoutItem item) {
            if (ContainsChild(item)) return;
            _children.Collection.Add(item);
            AppendChildInternal(item);
        }

        void ILayoutDriver.TruncateChild(ILayoutItem item) {
            if (!ContainsChild(item)) return;
            _children.Collection.Remove(item);
            TruncateChildInternal(item);
        }

        private bool ContainsChild(ILayoutItem item) {
            return _children.Any(x => x.Equals(item));
        }

        private void AppendChildInternal(ILayoutItem item) {
            AppendChild(item);
            item.LayoutDriver = this;
            item.ModifierUpdatedEvent += HandleChildModifierUpdated;
            RecalculateLayoutWithChildren();
            OnChildrenUpdated();
        }

        private void TruncateChildInternal(ILayoutItem item) {
            TruncateChild(item);
            item.LayoutDriver = null;
            item.ModifierUpdatedEvent -= HandleChildModifierUpdated;
            RecalculateLayoutWithChildren();
            OnChildrenUpdated();
        }

        private void TruncateChildrenInternal(IEnumerable<ILayoutItem> items) {
            foreach (var item in items) {
                TruncateChildInternal(item);
            }
        }

        private void HandleChildModifierUpdated(ILayoutItem item) {
            if (_beingRecalculated) return;
            //
            if (item.LayoutModifier == null) {
                RecalculateLayoutWithChildren();
            } else {
                ScheduleLayoutRecalculation();
            }
        }

        protected override void OnLayoutRefresh() {
            // called when wants to be recalculated
            if (_beingRecalculated) return;
            ScheduleLayoutRecalculation();
        }

        protected override void OnLayoutApply() {
            // called when layout driver updated object's dimensions
            if (LayoutDriver == null) return;
            RecalculateLayout();
        }

        #endregion

        #region Handle Children

        protected virtual Transform ChildrenContainer => ContentTransform;

        protected virtual void AppendChild(ILayoutItem item) {
            if (item is ReactiveComponentBase comp) {
                AppendReactiveChild(comp);
            } else {
                item.ApplyTransforms(x => x.SetParent(ChildrenContainer, false));
            }
        }

        protected virtual void TruncateChild(ILayoutItem item) {
            if (item is ReactiveComponentBase comp) {
                TruncateReactiveChild(comp);
            } else {
                item.ApplyTransforms(x => x.SetParent(null, false));
            }
        }

        protected virtual void AppendReactiveChild(ReactiveComponentBase comp) {
            comp.Use(ChildrenContainer);
        }

        protected virtual void TruncateReactiveChild(ReactiveComponentBase comp) {
            comp.Use();
        }

        #endregion

        #region Construct

        private class LayoutItemComparer : IEqualityComparer<ILayoutItem> {
            public bool Equals(ILayoutItem x, ILayoutItem y) => x.Equals(y);
            public int GetHashCode(ILayoutItem obj) => obj.GetHashCode();
        }

        private static readonly LayoutItemComparer layoutItemComparer = new();

        protected sealed override void ConstructInternal() {
            base.ConstructInternal();
            _children = new ObservableSet<ILayoutItem>(
                AppendChildInternal,
                TruncateChildInternal,
                TruncateChildrenInternal
            );
        }

        #endregion

        #region Overrides

        protected sealed override float? DesiredHeight => base.DesiredHeight;
        protected sealed override float? DesiredWidth => base.DesiredWidth;

        protected sealed override void OnModifierUpdatedInternal() {
            if (LayoutDriver == null) RecalculateLayoutInternal(true);
        }

        protected sealed override void OnLateUpdateInternal() {
            if (_recalculationScheduled) {
                RecalculateLayoutTree();
                _recalculationScheduled = false;
            }
            base.OnLateUpdateInternal();
        }

        #endregion

        #region Events

        protected virtual void OnChildrenUpdated() { }

        #endregion
    }
}