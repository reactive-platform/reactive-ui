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
                    _layoutController.RemoveAllChildren();
                    _layoutController.LayoutControllerUpdatedEvent -= ScheduleLayoutRecalculation;
                    ReleaseContextMember(_layoutController);
                }

                _layoutController = value;

                if (_layoutController != null) {
                    InsertContextMember(_layoutController);
                    _layoutController.LayoutControllerUpdatedEvent += ScheduleLayoutRecalculation;

                    var index = 0;
                    foreach (var child in _childrenOrdered) {
                        // Components without modifiers are not added to the hierarchy
                        if (child.LayoutModifier != null) {
                            _layoutController.InsertChild(child, index);
                            index++;
                        }
                    }
                }
            }
        }

        private ILayoutController? _layoutController;
        private bool _beingRecalculated;

        // Guard for requests from other components
        private float _lastRecalculationTime;

        private void RecalculateLayoutInternal() {
            var time = Time.time;
            // Used to prevent multiple recalculations
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (_lastRecalculationTime == time) {
                return;
            }

            if (_layoutController == null || Children.Count == 0) {
                return;
            }
            
            _layoutController.Recalculate(this);
            _layoutController.ApplyChildren();

            _lastRecalculationTime = Time.time;
        }

        public void RecalculateLayout() {
            _beingRecalculated = true;

            // Items without modifiers are not supposed to be controlled
            if (LayoutModifier != null && LayoutDriver?.LayoutController != null) {
                LayoutDriver.RecalculateLayout();

                _beingRecalculated = false;
                return;
            }

            RecalculateLayoutInternal();
            _beingRecalculated = false;
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

            if (_layoutController != null) {
                var index = _layoutController!.ChildCount;

                _layoutController.InsertChild(item, index);
            }

            ScheduleLayoutRecalculation();
            OnChildrenUpdated();
        }

        private void TruncateChildInternal(ILayoutItem item) {
            TruncatePhysicalChild(item);

            item.LayoutDriver = null;
            item.ModifierUpdatedEvent -= HandleChildModifierUpdated;
            _childrenOrdered.Remove(item);

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
            if (_beingRecalculated || _layoutController == null) {
                return;
            }

            var hasChild = _layoutController.HasChild(item);
            var hasModifications = false;

            if (item.LayoutModifier == null) {
                if (hasChild) {
                    // Here we remove the child from the layout controller exclusively to add it again later
                    _layoutController.RemoveChild(item);
                    hasModifications = true;
                }
            } else {
                if (!hasChild) {
                    var index = _childrenOrdered.FindLayoutItemIndex(item);
                    // It is crucial to maintain the same order as it can break the whole layout
                    _layoutController.InsertChild(item, index);
                    hasModifications = true;
                }
            }

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
            RecalculateLayout();
        }

        protected override void OnLayoutApply() {
            // If was applied outside the layout system
            if (LayoutDriver == null) {
                return;
            }

            if (_children.Count > 0) {
                _layoutController?.ApplyChildren();
            }
        }

        #endregion
    }
}