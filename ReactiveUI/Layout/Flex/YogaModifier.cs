using System;
using System.Diagnostics;
using JetBrains.Annotations;
using Reactive.Yoga;
using Debug = UnityEngine.Debug;

namespace Reactive {
    [PublicAPI]
    public class YogaModifier : LayoutModifierBase<YogaModifier> {
        #region Properties

        public PositionType PositionType {
            get => _positionType;
            set {
                _positionType = value;
                if (!HasValidNode) return;
                YogaNode.StyleSetPositionType(value);
                NotifyModifierUpdated();
            }
        }

        public Align AlignSelf {
            get => _alignSelf;
            set {
                _alignSelf = value;
                if (!HasValidNode) return;
                YogaNode.StyleSetAlignSelf(value);
                NotifyModifierUpdated();
            }
        }

        public YogaValue FlexBasis {
            get => _flexBasis;
            set {
                _flexBasis = value;
                if (!HasValidNode) return;
                YogaNode.StyleSetFlexBasis(value);
                NotifyModifierUpdated();
            }
        }

        public float FlexGrow {
            get => _flexGrow;
            set {
                _flexGrow = value;
                if (!HasValidNode) return;
                YogaNode.StyleSetFlexGrow(value);
                NotifyModifierUpdated();
            }
        }

        public float FlexShrink {
            get => _flexShrink;
            set {
                _flexShrink = value;
                if (!HasValidNode) return;
                YogaNode.StyleSetFlexShrink(value);
                NotifyModifierUpdated();
            }
        }

        public YogaFrame Position {
            get => _position;
            set {
                _position = value;
                if (!HasValidNode) return;
                RefreshPosition();
                NotifyModifierUpdated();
            }
        }

        public YogaVector Size {
            get => _size;
            set {
                _size = value;
                if (!HasValidNode) return;
                YogaNode.StyleSetWidth(_size.x);
                YogaNode.StyleSetHeight(_size.y);
                NotifyModifierUpdated();
            }
        }

        public YogaVector MinSize {
            get => _minSize;
            set {
                _minSize = value;
                if (!HasValidNode) return;
                YogaNode.StyleSetMinWidth(value.x);
                YogaNode.StyleSetMinHeight(value.y);
                NotifyModifierUpdated();
            }
        }

        public YogaVector MaxSize {
            get => _maxSize;
            set {
                _maxSize = value;
                if (!HasValidNode) return;
                YogaNode.StyleSetMaxWidth(value.x);
                YogaNode.StyleSetMaxHeight(value.y);
                NotifyModifierUpdated();
            }
        }

        public YogaValue AspectRatio {
            get => _aspectRatio;
            set {
                _aspectRatio = value;
                if (!HasValidNode) return;
                YogaNode.StyleSetAspectRatio(value.value);
                NotifyModifierUpdated();
            }
        }

        public YogaFrame Margin {
            get => _margin;
            set {
                _margin = value;
                if (!HasValidNode) return;
                RefreshMargin();
                NotifyModifierUpdated();
            }
        }

        private float _flexGrow;
        private float _flexShrink = 1;
        private Align _alignSelf = Align.Auto;
        private PositionType _positionType = PositionType.Relative;
        private YogaFrame _position = YogaFrame.Undefined;
        private YogaValue _flexBasis = YogaValue.Undefined;
        private YogaVector _size = YogaVector.Undefined;
        private YogaVector _minSize = YogaVector.Undefined;
        private YogaVector _maxSize = YogaVector.Undefined;
        private YogaValue _aspectRatio = YogaValue.Undefined;
        private YogaFrame _margin = YogaFrame.Zero;

        private void RefreshMargin() {
            YogaNode.StyleSetMargin(Edge.Top, _margin.top);
            YogaNode.StyleSetMargin(Edge.Bottom, _margin.bottom);
            YogaNode.StyleSetMargin(Edge.Left, _margin.left);
            YogaNode.StyleSetMargin(Edge.Right, _margin.right);
        }

        private void RefreshPosition() {
            YogaNode.StyleSetPosition(Edge.Top, _position.top);
            YogaNode.StyleSetPosition(Edge.Bottom, _position.bottom);
            YogaNode.StyleSetPosition(Edge.Left, _position.left);
            YogaNode.StyleSetPosition(Edge.Right, _position.right);
        }

        private void RefreshAllProperties() {
            YogaNode.StyleSetPositionType(_positionType);
            YogaNode.StyleSetAlignSelf(_alignSelf);
            YogaNode.StyleSetFlexBasis(_flexBasis);
            YogaNode.StyleSetFlexGrow(_flexGrow);
            YogaNode.StyleSetFlexShrink(_flexShrink);
            YogaNode.StyleSetMinWidth(_minSize.x);
            YogaNode.StyleSetMinHeight(_minSize.y);
            YogaNode.StyleSetMaxWidth(_maxSize.x);
            YogaNode.StyleSetMaxHeight(_maxSize.y);
            YogaNode.StyleSetAspectRatio(_aspectRatio.value);
            YogaNode.StyleSetWidth(_size.x);
            YogaNode.StyleSetHeight(_size.y);
            RefreshMargin();
            RefreshPosition();
        }

        #endregion

        #region Context

        public override Type ContextType { get; } = typeof(YogaContext);

        public override object CreateContext() => new YogaContext();

        public override void ProvideContext(object? context) {
            if (context != null) {
                _node = ((YogaContext)context).YogaNode;
                RefreshAllProperties();
                return;
            }

            if (HasValidNode) {
                // Clearing properties to prevent size lock on this node
                _minSize = YogaVector.Undefined;
                _maxSize = YogaVector.Undefined;
                _size = YogaVector.Undefined;
                _margin = YogaFrame.Undefined;
                _aspectRatio = YogaValue.Undefined;

                RefreshAllProperties();
                _node = null;
            }
        }

        #endregion

        #region Modifier

        internal YogaNode YogaNode {
            get {
                if (!(_node?.IsInitialized ?? false)) {
                    throw new Exception("Node was not initialized");
                }
                return _node;
            }
        }

        private bool HasValidNode => _node?.IsInitialized ?? false;

        private ILeafLayoutItem? _leafItem;
        private ILayoutItem? _item;
        private YogaNode? _node;

        public override void ExposeLayoutItem(ILayoutItem? item) {
            if (_item != null) {
                _item.StateUpdatedEvent -= HandleLayoutItemStateUpdated;

                if (_leafItem != null) {
                    _leafItem.LeafLayoutUpdatedEvent -= HandleLeafLayoutUpdated;

                    YogaNode.SetMeasureFunc(null);
                }
            }

            _item = item;
            _leafItem = item as ILeafLayoutItem;

            if (_item != null) {
                _item.StateUpdatedEvent += HandleLayoutItemStateUpdated;
                
                if (_leafItem != null) {
                    _leafItem.LeafLayoutUpdatedEvent += HandleLeafLayoutUpdated;

                    YogaNode.SetMeasureFunc(MeasureFuncWrapper);
                }
            }
        }

        private void HandleLayoutItemStateUpdated(ILayoutItem item) {
            YogaNode.StyleSetDisplay(item.WithinLayout ? Display.Flex : Display.None);
        }

        private void HandleLeafLayoutUpdated(ILeafLayoutItem item) {
            YogaNode.MarkDirty();
        }

        private YogaSize MeasureFuncWrapper(IntPtr node, float width, MeasureMode widthMode, float height, MeasureMode heightMode) {
            var size = _leafItem!.Measure(width, widthMode, height, heightMode);

            return new() {
                width = size.x,
                height = size.y
            };
        }

        public override void CopyFromSimilar(YogaModifier similar) {
            SuppressRefresh = true;
            PositionType = similar.PositionType;
            AlignSelf = similar.AlignSelf;
            FlexBasis = similar.FlexBasis;
            FlexGrow = similar.FlexGrow;
            FlexShrink = similar.FlexShrink;
            Position = similar.Position;
            Size = similar.Size;
            MinSize = similar.MinSize;
            MaxSize = similar.MinSize;
            AspectRatio = similar.AspectRatio;
            Margin = similar.Margin;
            SuppressRefresh = false;
            NotifyModifierUpdated();
        }

        #endregion
    }
}