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

                    foreach (var child in Children) {
                        var index = _layoutController.ChildCount;
                        // Components without modifiers are not added to the hierarchy, so instead of
                        // just incrementing the index each cycle, we rely on the actual child count
                        _layoutController.InsertChild(child, index);
                    }
                }
            }
        }

        private ILayoutController? _layoutController;
        private bool _beingRecalculated;
        private bool _recalculationScheduled;

        // Guard for requests from other components
        private float _lastRecalculationTime;

        // Guard for requests from itself
        private bool _recalculatedThisFrame;

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

            _layoutController.PrepareForRecalculation();
            _layoutController.Recalculate(this);
            _layoutController.ApplyChildren();

            _lastRecalculationTime = Time.time;
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

            RecalculateLayoutInternal();
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

        private void AppendChildInternal(ILayoutItem item) {
            AppendPhysicalChild(item);

            item.LayoutDriver = this;
            item.ModifierUpdatedEvent += HandleChildModifierUpdated;

            if (_layoutController != null) {
                var index = _layoutController!.ChildCount - 1;

                if (index < 0) {
                    index = 0;
                }

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

        protected sealed override void OnLateUpdate() {
            if (_recalculationScheduled) {
                RecalculateLayout();
                _recalculationScheduled = false;
            }
        }

        protected override void OnRecalculateLayoutSelf() {
            RecalculateLayout();
            // LateUpdate is called right after this method, so we prevent
            // the layout from starting a second layout recalculation 
            _recalculationScheduled = false;
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