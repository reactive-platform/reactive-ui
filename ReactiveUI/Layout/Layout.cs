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
            get {
                ComponentDefaults.LayoutController.AssignOptional(ref _layoutController);
                return _layoutController.Value;
            }
            set {
                var oldController = _layoutController.Value;
                
                if (oldController != null) {
                    oldController.RemoveAllChildren();
                    oldController.LayoutControllerUpdatedEvent -= ScheduleLayoutRecalculation;
                    ReleaseContextMember(oldController);
                }

                _layoutController = Optional<ILayoutController?>.Some(value);

                if (value != null) {
                    InsertContextMember(value);
                    value.LayoutControllerUpdatedEvent += ScheduleLayoutRecalculation;

                    var index = 0;
                    foreach (var child in _childrenOrdered) {
                        // Components without modifiers are not added to the hierarchy
                        if (child.LayoutModifier != null) {
                            value.InsertChild(child, index);
                            index++;
                        }
                    }
                }
            }
        }

        private Optional<ILayoutController?> _layoutController;
        private bool _beingRecalculated;

        // Guard for requests from other components
        private float _lastRecalculationTime;

        private void RecalculateLayoutInternal() {
            var layoutController = LayoutController;
            var time = Time.time;
            // Used to prevent multiple recalculations
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (_lastRecalculationTime == time) {
                return;
            }

            if (layoutController == null || Children.Count == 0) {
                return;
            }

            Vector2 constraints;
            if (ContentTransform.parent is RectTransform parent) {
                constraints = parent.rect.size;
            } else {
                constraints = new Vector2(float.NaN, float.NaN);
            }
            
            layoutController.Recalculate(this, constraints);
            layoutController.ApplyChildren();

            _lastRecalculationTime = Time.time;
        }

        public void RecalculateLayoutImmediate() {
            _beingRecalculated = true;

            // Items without modifiers are not supposed to be controlled
            if (LayoutModifier != null && LayoutDriver?.LayoutController != null) {
                LayoutDriver.RecalculateLayoutImmediate();

                _beingRecalculated = false;
                return;
            }

            RecalculateLayoutInternal();
            _beingRecalculated = false;
        }

        public new void ScheduleLayoutRecalculation() {
            base.ScheduleLayoutRecalculation();
        }

        #endregion

        #region Children

        /// <summary>
        /// Represents the children of the component. This collection is observable and can be safely used for children modification.
        /// </summary>
        public ICollection<ILayoutItem> Children { get; private set; } = null!;

        private LayoutSet _children = new();
        private List<ILayoutItem> _childrenOrdered = new();

        private void AppendChildInternal(ILayoutItem item) {
            AppendPhysicalChild(item);

            item.LayoutDriver = this;
            item.ModifierUpdatedEvent += HandleChildModifierUpdated;
            _childrenOrdered.Add(item);

            var layoutController = LayoutController;
            if (layoutController != null) {
                var index = layoutController.ChildCount;

                layoutController.InsertChild(item, index);
            }

            ScheduleLayoutRecalculation();
            OnChildrenUpdated();
        }

        private void TruncateChildInternal(ILayoutItem item) {
            TruncatePhysicalChild(item);

            item.LayoutDriver = null;
            item.ModifierUpdatedEvent -= HandleChildModifierUpdated;
            _childrenOrdered.Remove(item);

            LayoutController?.RemoveChild(item);
            ScheduleLayoutRecalculation();

            OnChildrenUpdated();
        }

        private void TruncateAllChildrenInternal(IEnumerable<ILayoutItem> items) {
            foreach (var item in items) {
                TruncateChildInternal(item);
            }
        }

        private void HandleChildModifierUpdated(ILayoutItem item) {
            var layoutController = LayoutController;
            if (_beingRecalculated || layoutController == null) {
                return;
            }

            var hasChild = layoutController.HasChild(item);

            if (item.LayoutModifier == null) {
                if (hasChild) {
                    // Here we remove the child from the layout controller exclusively to add it again later
                    layoutController.RemoveChild(item);
                }
            } else {
                if (!hasChild) {
                    var index = _childrenOrdered.FindLayoutItemIndex(item);
                    // It is crucial to maintain the same order as it can break the whole layout
                    layoutController.InsertChild(item, index);
                }
            }

            ScheduleLayoutRecalculation();
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
            if (LayoutDriver == null && !_beingRecalculated) {
                ScheduleLayoutRecalculation();
            }
        }

        protected override void OnRecalculateLayoutSelf() {
            RecalculateLayoutImmediate();
        }

        protected override void OnLayoutApply() {
            // If was applied outside the layout system
            if (LayoutDriver == null) {
                return;
            }

            if (_children.Count > 0) {
                LayoutController?.ApplyChildren();
            }
        }

        #endregion
    }
}