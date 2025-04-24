using System;
using JetBrains.Annotations;

namespace Reactive.Yoga {
    [PublicAPI]
    public class YogaLayoutController : ILayoutController {
        #region Properties

        public Overflow Overflow {
            get => HasValidNode ? YogaNode.StyleGetOverflow() : _overflow;
            set {
                _overflow = value;
                _changedCacheBeforeInit = true;

                if (HasValidNode) {
                    YogaNode.StyleSetOverflow(_overflow);
                    NotifyControllerUpdated();
                }
            }
        }

        public Direction Direction {
            get => HasValidNode ? YogaNode.StyleGetDirection() : _direction;
            set {
                _direction = value;
                _changedCacheBeforeInit = true;

                if (HasValidNode) {
                    YogaNode.StyleSetDirection(_direction);
                    NotifyControllerUpdated();
                }
            }
        }

        public FlexDirection FlexDirection {
            get => HasValidNode ? YogaNode.StyleGetFlexDirection() : _flexDirection;
            set {
                _flexDirection = value;
                _changedCacheBeforeInit = true;

                if (HasValidNode) {
                    YogaNode.StyleSetFlexDirection(_flexDirection);
                    NotifyControllerUpdated();
                }
            }
        }

        public Wrap FlexWrap {
            get => HasValidNode ? YogaNode.StyleGetFlexWrap() : _flexWrap;
            set {
                _flexWrap = value;
                _changedCacheBeforeInit = true;

                if (HasValidNode) {
                    YogaNode.StyleSetFlexWrap(_flexWrap);
                    NotifyControllerUpdated();
                }
            }
        }

        public Justify JustifyContent {
            get => HasValidNode ? YogaNode.StyleGetJustifyContent() : _justifyContent;
            set {
                _justifyContent = value;
                _changedCacheBeforeInit = true;

                if (HasValidNode) {
                    YogaNode.StyleSetJustifyContent(_justifyContent);
                    NotifyControllerUpdated();
                }
            }
        }

        public Align AlignItems {
            get => HasValidNode ? YogaNode.StyleGetAlignItems() : _alignItems;
            set {
                _alignItems = value;
                _changedCacheBeforeInit = true;

                if (HasValidNode) {
                    YogaNode.StyleSetAlignItems(_alignItems);
                    NotifyControllerUpdated();
                }
            }
        }

        public Align AlignContent {
            get => HasValidNode ? YogaNode.StyleGetAlignContent() : _alignContent;
            set {
                _alignContent = value;
                _changedCacheBeforeInit = true;

                if (HasValidNode) {
                    YogaNode.StyleSetAlignContent(_alignContent);
                    NotifyControllerUpdated();
                }
            }
        }

        public YogaFrame Padding {
            get {
                if (HasValidNode) {
                    return new() {
                        top = YogaNode.StyleGetPadding(Edge.Top),
                        bottom = YogaNode.StyleGetPadding(Edge.Bottom),
                        left = YogaNode.StyleGetPadding(Edge.Left),
                        right = YogaNode.StyleGetPadding(Edge.Right)
                    };
                }

                return _padding;
            }
            set {
                _padding = value;
                _changedCacheBeforeInit = true;

                if (HasValidNode) {
                    RefreshPadding();
                    NotifyControllerUpdated();
                }
            }
        }

        public YogaVector Gap {
            get {
                if (HasValidNode) {
                    return new() {
                        x = YogaNode.StyleGetGap(Gutter.Column),
                        y = YogaNode.StyleGetGap(Gutter.Row)
                    };
                }

                return _gap;
            }
            set {
                _gap = value;
                _changedCacheBeforeInit = true;

                if (HasValidNode) {
                    RefreshGap();
                    NotifyControllerUpdated();
                }
            }
        }
        
        public bool HadOverflow => HasValidNode && YogaNode.HadOverflow();

        public event Action? LayoutControllerUpdatedEvent;

        private Overflow _overflow;
        private Direction _direction;
        private FlexDirection _flexDirection;
        private Wrap _flexWrap;
        private Justify _justifyContent;
        private Align _alignItems;
        private Align _alignContent;
        private YogaFrame _padding;
        private YogaVector _gap;
        private bool _changedCacheBeforeInit;

        private void NotifyControllerUpdated() {
            LayoutControllerUpdatedEvent?.Invoke();
        }

        private void RefreshPadding() {
            if (_padding.top == _padding.bottom) {
                YogaNode.StyleSetPadding(Edge.Vertical, _padding.top);
            } else {
                YogaNode.StyleSetPadding(Edge.Top, _padding.top);
                YogaNode.StyleSetPadding(Edge.Bottom, _padding.bottom);
            }

            if (_padding.left == _padding.right) {
                YogaNode.StyleSetPadding(Edge.Horizontal, _padding.left);
            } else {
                YogaNode.StyleSetPadding(Edge.Left, _padding.left);
                YogaNode.StyleSetPadding(Edge.Right, _padding.right);
            }
        }

        private void RefreshGap() {
            YogaNode.StyleSetGap(Gutter.Column, _gap.x);
            YogaNode.StyleSetGap(Gutter.Row, _gap.y);
        }

        private void RefreshAllProperties() {
            YogaNode.StyleSetOverflow(_overflow);
            YogaNode.StyleSetDirection(_direction);
            YogaNode.StyleSetFlexDirection(_flexDirection);
            YogaNode.StyleSetFlexWrap(_flexWrap);
            YogaNode.StyleSetJustifyContent(_justifyContent);
            YogaNode.StyleSetAlignItems(_alignItems);
            YogaNode.StyleSetAlignContent(_alignContent);
            RefreshGap();
            RefreshPadding();
        }

        #endregion

        #region Context

        public Type ContextType { get; } = typeof(YogaContext);

        private bool _everInitialized;

        public object CreateContext() => new YogaContext();

        public void ProvideContext(object? context) {
            if (context == null) {
                _contextNode = null;
                return;
            }

            if (_everInitialized) {
                throw new InvalidOperationException("YogaLayoutController can be used only once.");
            }

            _contextNode = ((YogaContext)context).YogaNode;
            _everInitialized = true;

            if (_changedCacheBeforeInit) {
                RefreshAllProperties();
            }
        }

        #endregion

        #region Calculations

        private bool HasValidNode => _contextNode?.IsInitialized ?? false;

        private YogaNode YogaNode {
            get {
                if (!HasValidNode) {
                    throw new Exception("Node was not initialized");
                }
                return _contextNode!;
            }
        }

        private YogaNode? _contextNode;

        public void Recalculate(ILayoutItem item) {
            var transform = item.BeginApply();
            var rect = transform.rect;

            YogaNode.CalculateLayout(rect.width, rect.height, Direction);

            if (YogaNode.GetHasNewLayout()) {
                // Root nodes don't need to apply position
                YogaNode.ApplySizeTo(transform);
            }

            item.EndApply();
        }

        #endregion

        #region Children

        public int ChildCount => _nodes.Count;

        private readonly LayoutDictionary<YogaNode> _nodes = new();

        public void InsertChild(ILayoutItem comp, int index) {
            if (comp.LayoutModifier is not YogaModifier modifier) {
                return;
            }

            if (_nodes.ContainsKey(comp)) {
                return;
            }

            YogaNode.InsertChild(modifier.YogaNode, index);

            _nodes.Add(comp, modifier.YogaNode);
        }

        public void RemoveChild(ILayoutItem comp) {
            if (!_nodes.TryGetValue(comp, out var node)) {
                return;
            }

            YogaNode.RemoveChild(node);

            _nodes.Remove(comp);
        }

        public void RemoveAllChildren() {
            YogaNode.RemoveAllChildren();
            _nodes.Clear();
        }

        public bool HasChild(ILayoutItem comp) {
            return _nodes.ContainsKey(comp);
        }

        public void ApplyChildren() {
            if (!YogaNode.GetHasNewLayout()) {
                return;
            }

            YogaNode.SetHasNewLayout(false);

            foreach (var (child, node) in _nodes) {
                var rect = child.BeginApply();
                node.ApplyTo(rect);
                child.EndApply();
            }
        }

        #endregion
    }
}