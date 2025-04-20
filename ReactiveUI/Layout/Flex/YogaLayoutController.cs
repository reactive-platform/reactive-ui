using System;

namespace Reactive.Yoga {
    public class YogaLayoutController : ILayoutController {
        #region Properties

        public Overflow Overflow {
            get => _overflow;
            set {
                _overflow = value;
                YogaNode.StyleSetOverflow(_overflow);
                NotifyControllerUpdated();
            }
        }

        public Direction Direction {
            get => _direction;
            set {
                _direction = value;
                YogaNode.StyleSetDirection(_direction);
                NotifyControllerUpdated();
            }
        }

        public FlexDirection FlexDirection {
            get => _flexDirection;
            set {
                _flexDirection = value;
                YogaNode.StyleSetFlexDirection(_flexDirection);
                NotifyControllerUpdated();
            }
        }

        public Wrap FlexWrap {
            get => _flexWrap;
            set {
                _flexWrap = value;
                YogaNode.StyleSetFlexWrap(_flexWrap);
                NotifyControllerUpdated();
            }
        }

        public Justify JustifyContent {
            get => _justifyContent;
            set {
                _justifyContent = value;
                YogaNode.StyleSetJustifyContent(_justifyContent);
                NotifyControllerUpdated();
            }
        }

        public Align AlignItems {
            get => _alignItems;
            set {
                _alignItems = value;
                YogaNode.StyleSetAlignItems(_alignItems);
                NotifyControllerUpdated();
            }
        }

        public Align AlignContent {
            get => _alignContent;
            set {
                _alignContent = value;
                YogaNode.StyleSetAlignContent(_alignContent);
                NotifyControllerUpdated();
            }
        }

        public YogaFrame Padding {
            get => _padding;
            set {
                _padding = value;
                RefreshPadding();
                NotifyControllerUpdated();
            }
        }

        public YogaVector Gap {
            get => _gap;
            set {
                _gap = value;
                RefreshGap();
                NotifyControllerUpdated();
            }
        }

        public event Action? LayoutControllerUpdatedEvent;

        private Overflow _overflow = Overflow.Visible;
        private Direction _direction = Direction.Inherit;
        private FlexDirection _flexDirection = FlexDirection.Row;
        private Justify _justifyContent = Justify.FlexStart;
        private Align _alignItems = Align.FlexStart;
        private Align _alignContent = Align.Auto;
        private Wrap _flexWrap = Wrap.Wrap;
        private YogaFrame _padding = YogaFrame.Zero;
        private YogaVector _gap = YogaVector.Undefined;

        private void NotifyControllerUpdated() {
            LayoutControllerUpdatedEvent?.Invoke();
        }

        private void RefreshGap() {
            YogaNode.StyleSetGap(Gutter.Row, _gap.y);
            YogaNode.StyleSetGap(Gutter.Column, _gap.x);
        }

        private void RefreshPadding() {
            YogaNode.StyleSetPadding(Edge.Top, _padding.top);
            YogaNode.StyleSetPadding(Edge.Bottom, _padding.bottom);
            YogaNode.StyleSetPadding(Edge.Left, _padding.left);
            YogaNode.StyleSetPadding(Edge.Right, _padding.right);
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

        public object CreateContext() => new YogaContext();

        public void ProvideContext(object? context) {
            if (context == null) {
                if (_contextNode.GetIsInitialized()) {
                    _contextNode!.RemoveAllChildren();
                }
                _contextNode = default;
                return;
            }

            var c = (YogaContext)context;
            _contextNode = c.YogaNode;
            RefreshAllProperties();
        }

        #endregion

        #region Calculations

        private YogaNode YogaNode {
            get {
                if (_contextNode is not { IsInitialized: true }) {
                    throw new Exception("Node was not initialized");
                }

                return _contextNode;
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