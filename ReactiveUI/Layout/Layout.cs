using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Reactive {
    /// <summary>
    /// A component that can control the layout inside it.
    /// </summary>
    [PublicAPI]
    public class Layout : ReactiveComponent, ILayoutDriver {
        #region Layout Controller

        public ILayoutController? LayoutController {
            get => _layoutController;
            set {
                if (_layoutController != null) {
                    ReleaseContextMember(_layoutController);
                    _layoutController.LayoutControllerUpdatedEvent -= ScheduleLayoutRecalculation;
                    _layoutController.RemoveAllChildren();
                }

                _layoutController = value;

                if (_layoutController != null) {
                    InsertContextMember(_layoutController);
                    _layoutController.LayoutControllerUpdatedEvent += ScheduleLayoutRecalculation;

                    var i = 0;
                    foreach (var child in Children) {
                        _layoutController.InsertChild(child, i);
                        i++;
                    }
                }
            }
        }

        private ILayoutController? _layoutController;
        private bool _beingRecalculated;
        private bool _recalculationScheduled;

        private void RecalculateLayoutInternal(bool root) {
            if (_layoutController == null || Children.Count == 0) {
                return;
            }

            _layoutController.PrepareForRecalculation();
            _layoutController.Recalculate(this);
            _layoutController.ApplyChildren();
        }

        public void RecalculateLayout() {
            _beingRecalculated = true;

            // Items without modifiers are not supposed to be controlled
            if (LayoutModifier != null && LayoutDriver?.LayoutController != null) {
                _layoutController?.PrepareForRecalculation();
                LayoutDriver.RecalculateLayout();

                _beingRecalculated = false;
                return;
            }

            RecalculateLayoutInternal(true);
            _beingRecalculated = false;
        }

        private void ScheduleLayoutRecalculation() {
            _recalculationScheduled = true;
        }

        #endregion

        #region Children

        /// <summary>
        /// Represents the children of the component. This collection is observable and can be safely used for children modification.
        /// </summary>
        public ICollection<ILayoutItem> Children { get; private set; } = null!;

        private HashSet<ILayoutItem> _children = new();

        public void AppendChild(ILayoutItem item) {
            if (!_children.Add(item)) {
                return;
            }

            AppendChildInternal(item);
        }

        public void TruncateChild(ILayoutItem item) {
            if (!_children.Remove(item)) {
                return;
            }

            TruncateChildInternal(item);
        }

        private void AppendChildInternal(ILayoutItem item) {
            AppendPhysicalChild(item);

            item.LayoutDriver = this;
            item.ModifierUpdatedEvent += HandleChildModifierUpdated;

            if (_layoutController != null) {
                var index = _layoutController!.ChildCount - 1;
                _layoutController.InsertChild(item, index);
            }
            ScheduleLayoutRecalculation();

            OnChildrenUpdated();
        }

        private void TruncateChildInternal(ILayoutItem item) {
            TruncatePhysicalChild(item);

            item.LayoutDriver = null;
            item.ModifierUpdatedEvent -= HandleChildModifierUpdated;

            _layoutController?.RemoveChild(item);
            ScheduleLayoutRecalculation();

            OnChildrenUpdated();
        }

        private void TruncateAllChildrenInternal(IEnumerable<ILayoutItem> items) {
            foreach (var item in items) {
                TruncateChildInternal(item);
            }
        }

        private void HandleChildModifierUpdated(ILayoutItem item) {
            if (_beingRecalculated) {
                return;
            }

            var hasModifications = item.LayoutModifier == null ?
                _children.Remove(item) :
                _children.Add(item);

            if (hasModifications) {
                ScheduleLayoutRecalculation();
            }
        }

        protected virtual void OnChildrenUpdated() { }

        #endregion

        #region Physical Children

        /// <summary>
        /// Determines a physical container for the children to be reparented to.
        /// </summary>
        protected virtual Transform PhysicalContainer => ContentTransform;

        protected virtual void AppendPhysicalChild(ILayoutItem item) {
            if (item is IReactiveComponent comp) {
                comp.Use(PhysicalContainer);
            } else {
                item.BeginApply().SetParent(PhysicalContainer, false);
                item.EndApply();
            }
        }

        protected virtual void TruncatePhysicalChild(ILayoutItem item) {
            if (item is IReactiveComponent comp) {
                comp.Use(null);
            } else {
                item.BeginApply().SetParent(null, false);
                item.EndApply();
            }
        }

        #endregion

        #region Construct

        private class LayoutItemComparer : IEqualityComparer<ILayoutItem> {
            public bool Equals(ILayoutItem x, ILayoutItem y) => x.Equals(y);
            public int GetHashCode(ILayoutItem obj) => obj.GetHashCode();
        }

        private static readonly LayoutItemComparer layoutItemComparer = new();

        protected override void OnInstantiate() {
            Children = new ObservableCollectionAdapter<ILayoutItem>(
                _children,
                AppendChildInternal,
                TruncateChildInternal,
                TruncateAllChildrenInternal
            );
        }

        #endregion

        #region Overrides

        protected sealed override void OnModifierUpdated() {
            if (LayoutDriver == null) {
                RecalculateLayoutInternal(true);
            }
        }

        protected sealed override void OnLateUpdate() {
            if (_recalculationScheduled) {
                RecalculateLayout();
                _recalculationScheduled = false;
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
    }
}