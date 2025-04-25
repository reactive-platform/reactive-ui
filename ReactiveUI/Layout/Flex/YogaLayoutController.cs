using System;
using JetBrains.Annotations;

namespace Reactive.Yoga {
    [PublicAPI]
    public class YogaLayoutController : ILayoutController {
        #region Properties

        public Overflow Overflow {
            get => HasValidNode ? YogaNode.StyleGetOverflow() : _overflow.GetValueOrDefault();
            set {
                _overflow = value;
                _changedCacheBeforeInit = true;

                if (HasValidNode) {
                    YogaNode.StyleSetOverflow(_overflow.Value);
                    NotifyControllerUpdated();
                }
            }
        }

        public Direction Direction {
            get => HasValidNode ? YogaNode.StyleGetDirection() : _direction.GetValueOrDefault();
            set {
                _direction = value;
                _changedCacheBeforeInit = true;

                if (HasValidNode) {
                    YogaNode.StyleSetDirection(_direction.Value);
                    NotifyControllerUpdated();
                }
            }
        }

        public FlexDirection FlexDirection {
            get => HasValidNode ? YogaNode.StyleGetFlexDirection() : _flexDirection.GetValueOrDefault();
            set {
                _flexDirection = value;
                _changedCacheBeforeInit = true;

                if (HasValidNode) {
                    YogaNode.StyleSetFlexDirection(_flexDirection.Value);
                    NotifyControllerUpdated();
                }
            }
        }

        public Wrap FlexWrap {
            get => HasValidNode ? YogaNode.StyleGetFlexWrap() : _flexWrap.GetValueOrDefault();
            set {
                _flexWrap = value;
                _changedCacheBeforeInit = true;

                if (HasValidNode) {
                    YogaNode.StyleSetFlexWrap(_flexWrap.Value);
                    NotifyControllerUpdated();
                }
            }
        }

        public Justify JustifyContent {
            get => HasValidNode ? YogaNode.StyleGetJustifyContent() : _justifyContent.GetValueOrDefault();
            set {
                _justifyContent = value;
                _changedCacheBeforeInit = true;

                if (HasValidNode) {
                    YogaNode.StyleSetJustifyContent(_justifyContent.Value);
                    NotifyControllerUpdated();
                }
            }
        }

        public Align AlignItems {
            get => HasValidNode ? YogaNode.StyleGetAlignItems() : _alignItems.GetValueOrDefault();
            set {
                _alignItems = value;
                _changedCacheBeforeInit = true;

                if (HasValidNode) {
                    YogaNode.StyleSetAlignItems(_alignItems.Value);
                    NotifyControllerUpdated();
                }
            }
        }

        public Align AlignContent {
            get => HasValidNode ? YogaNode.StyleGetAlignContent() : _alignContent.GetValueOrDefault();
            set {
                _alignContent = value;
                _changedCacheBeforeInit = true;

                if (HasValidNode) {
                    YogaNode.StyleSetAlignContent(_alignContent.Value);
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

                return _padding.GetValueOrDefault();
            }
            set {
                _padding = value;
                _changedCacheBeforeInit = true;

                if (HasValidNode) {
                    RefreshPadding(value);
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

                return _gap.GetValueOrDefault();
            }
            set {
                _gap = value;
                _changedCacheBeforeInit = true;

                if (HasValidNode) {
                    RefreshGap(value);
                    NotifyControllerUpdated();
                }
            }
        }

        public bool HadOverflow => HasValidNode && YogaNode.HadOverflow();

        public event Action? LayoutControllerUpdatedEvent;

        private Overflow? _overflow;
        private Direction? _direction;
        private FlexDirection? _flexDirection;
        private Wrap? _flexWrap;
        private Justify? _justifyContent;
        private Align? _alignItems;
        private Align? _alignContent;
        private YogaFrame? _padding;
        private YogaVector? _gap;
        private bool _changedCacheBeforeInit;

        private void NotifyControllerUpdated() {
            LayoutControllerUpdatedEvent?.Invoke();
        }

        private void RefreshPadding(YogaFrame value) {
            if (value.top == value.bottom) {
                YogaNode.StyleSetPadding(Edge.Vertical, value.top);
            } else {
                YogaNode.StyleSetPadding(Edge.Top, value.top);
                YogaNode.StyleSetPadding(Edge.Bottom, value.bottom);
            }

            if (value.left == value.right) {
                YogaNode.StyleSetPadding(Edge.Horizontal, value.left);
            } else {
                YogaNode.StyleSetPadding(Edge.Left, value.left);
                YogaNode.StyleSetPadding(Edge.Right, value.right);
            }
        }

        private void RefreshGap(YogaVector value) {
            YogaNode.StyleSetGap(Gutter.Column, value.x);
            YogaNode.StyleSetGap(Gutter.Row, value.y);
        }

        private void RefreshAllProperties() {
            if (_overflow.HasValue) {
                YogaNode.StyleSetOverflow(_overflow.Value);
            }
            if (_direction.HasValue) {
                YogaNode.StyleSetDirection(_direction.Value);
            }
            if (_flexDirection.HasValue) {
                YogaNode.StyleSetFlexDirection(_flexDirection.Value);
            }
            if (_flexWrap.HasValue) {
                YogaNode.StyleSetFlexWrap(_flexWrap.Value);
            }
            if (_justifyContent.HasValue) {
                YogaNode.StyleSetJustifyContent(_justifyContent.Value);
            }
            if (_alignItems.HasValue) {
                YogaNode.StyleSetAlignItems(_alignItems.Value);
            }
            if (_alignContent.HasValue) {
                YogaNode.StyleSetAlignContent(_alignContent.Value);
            }
            if (_gap.HasValue) {
                RefreshGap(_gap.Value);
            }
            if (_padding.HasValue) {
                RefreshPadding(_padding.Value);
            }
        }
        
        #endregion

        #region Context

        public Type ContextType { get; } = typeof(YogaContext);

        public object CreateContext() => new YogaContext();

        public void ProvideContext(object? context) {
            if (context == null) {
                _node = null;
                return;
            }

            _node = ((YogaContext)context).YogaNode;

            if (_changedCacheBeforeInit) {
                RefreshAllProperties();
            }
        }

        #endregion

        #region Calculations

        private bool HasValidNode => _node?.IsInitialized ?? false;

        private YogaNode YogaNode {
            get {
                if (!HasValidNode) {
                    throw new Exception("Node was not initialized");
                }
                return _node!;
            }
        }

        private YogaNode? _node;

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