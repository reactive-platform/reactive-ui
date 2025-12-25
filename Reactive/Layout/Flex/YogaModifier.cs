using System;
using JetBrains.Annotations;

namespace Reactive.Yoga;

[PublicAPI]
public class YogaModifier : LayoutModifierBase<YogaModifier> {
    #region Properties

    public PositionType PositionType {
        get => HasValidNode ? YogaNode.StyleGetPositionType() : _positionType.GetValueOrDefault();
        set {
            _positionType = value;
            _changedCacheBeforeInit = true;

            if (HasValidNode) {
                YogaNode.StyleSetPositionType(value);
                NotifyModifierUpdated();
            }
        }
    }

    public Align AlignSelf {
        get => HasValidNode ? YogaNode.StyleGetAlignSelf() : _alignSelf.GetValueOrDefault();
        set {
            _alignSelf = value;
            _changedCacheBeforeInit = true;

            if (HasValidNode) {
                YogaNode.StyleSetAlignSelf(value);
                NotifyModifierUpdated();
            }
        }
    }

    public YogaValue FlexBasis {
        get => HasValidNode ? YogaNode.StyleGetFlexBasis() : _flexBasis.GetValueOrDefault();
        set {
            _flexBasis = value;
            _changedCacheBeforeInit = true;

            if (HasValidNode) {
                YogaNode.StyleSetFlexBasis(value);
                NotifyModifierUpdated();
            }
        }
    }

    public float FlexGrow {
        get => HasValidNode ? YogaNode.StyleGetFlexGrow() : _flexGrow.GetValueOrDefault();
        set {
            _flexGrow = value;
            _changedCacheBeforeInit = true;

            if (HasValidNode) {
                YogaNode.StyleSetFlexGrow(value);
                NotifyModifierUpdated();
            }
        }
    }

    public float FlexShrink {
        get => HasValidNode ? YogaNode.StyleGetFlexShrink() : _flexShrink.GetValueOrDefault();
        set {
            _flexShrink = value;
            _changedCacheBeforeInit = true;

            if (HasValidNode) {
                YogaNode.StyleSetFlexShrink(value);
                NotifyModifierUpdated();
            }
        }
    }

    public float Flex {
        get => HasValidNode ? YogaNode.StyleGetFlex() : _flex.GetValueOrDefault();
        set {
            _flex = value;
            _changedCacheBeforeInit = true;

            if (HasValidNode) {
                YogaNode.StyleSetFlex(value);
                NotifyModifierUpdated();
            }
        }
    }

    public YogaFrame Position {
        get {
            if (HasValidNode) {
                return new() {
                    top = YogaNode.StyleGetPosition(Edge.Top),
                    bottom = YogaNode.StyleGetPosition(Edge.Bottom),
                    left = YogaNode.StyleGetPosition(Edge.Left),
                    right = YogaNode.StyleGetPosition(Edge.Right)
                };
            }
            return _position.GetValueOrDefault();
        }
        set {
            _position = value;
            _changedCacheBeforeInit = true;

            if (HasValidNode) {
                RefreshPosition(value);
                NotifyModifierUpdated();
            }
        }
    }

    public YogaVector Size {
        get {
            if (HasValidNode) {
                return new() {
                    x = YogaNode.StyleGetWidth(),
                    y = YogaNode.StyleGetHeight()
                };
            }
            return _size.GetValueOrDefault();
        }
        set {
            _size = value;
            _changedCacheBeforeInit = true;

            if (HasValidNode) {
                YogaNode.StyleSetWidth(value.x);
                YogaNode.StyleSetHeight(value.y);
                NotifyModifierUpdated();
            }
        }
    }

    public YogaVector MinSize {
        get {
            if (HasValidNode) {
                return new() {
                    x = YogaNode.StyleGetMinWidth(),
                    y = YogaNode.StyleGetMinHeight()
                };
            }
            return _minSize.GetValueOrDefault();
        }
        set {
            _minSize = value;
            _changedCacheBeforeInit = true;

            if (HasValidNode) {
                YogaNode.StyleSetMinWidth(value.x);
                YogaNode.StyleSetMinHeight(value.y);
                NotifyModifierUpdated();
            }
        }
    }

    public YogaVector MaxSize {
        get {
            if (HasValidNode) {
                return new() {
                    x = YogaNode.StyleGetMaxWidth(),
                    y = YogaNode.StyleGetMaxHeight()
                };
            }
            return _maxSize.GetValueOrDefault();
        }
        set {
            _maxSize = value;
            _changedCacheBeforeInit = true;

            if (HasValidNode) {
                YogaNode.StyleSetMaxWidth(value.x);
                YogaNode.StyleSetMaxHeight(value.y);
                NotifyModifierUpdated();
            }
        }
    }

    public float AspectRatio {
        get => HasValidNode ? YogaNode.StyleGetAspectRatio() : _aspectRatio.GetValueOrDefault();
        set {
            _aspectRatio = value;
            _changedCacheBeforeInit = true;

            if (HasValidNode) {
                YogaNode.StyleSetAspectRatio(value);
                NotifyModifierUpdated();
            }
        }
    }

    public YogaFrame Margin {
        get {
            if (HasValidNode) {
                return new() {
                    top = YogaNode.StyleGetMargin(Edge.Top),
                    bottom = YogaNode.StyleGetMargin(Edge.Bottom),
                    left = YogaNode.StyleGetMargin(Edge.Left),
                    right = YogaNode.StyleGetMargin(Edge.Right)
                };
            }
            return _margin.GetValueOrDefault();
        }
        set {
            _margin = value;
            _changedCacheBeforeInit = true;

            if (HasValidNode) {
                RefreshMargin(value);
                NotifyModifierUpdated();
            }
        }
    }

    public bool HadOverflow => HasValidNode && YogaNode.HadOverflow();

    // Take into account that yoga can modify values on the backend, so when debugging,
    // do NOT rely on these variables, rely on properties instead.
    // These variables hold cached values for late initialization and are not intended for anything else.
    private PositionType? _positionType;
    private Align? _alignSelf;
    private float? _aspectRatio;
    private float? _flex;
    private float? _flexGrow;
    private float? _flexShrink;
    private YogaValue? _flexBasis;
    private YogaFrame? _position;
    private YogaVector? _size;
    private YogaVector? _minSize;
    private YogaVector? _maxSize;
    private YogaFrame? _margin;
    private bool _changedCacheBeforeInit;

    private void RefreshMargin(YogaFrame value) {
        YogaNode.StyleSetMargin(Edge.Top, value.top);
        YogaNode.StyleSetMargin(Edge.Bottom, value.bottom);
        YogaNode.StyleSetMargin(Edge.Left, value.left);
        YogaNode.StyleSetMargin(Edge.Right, value.right);
    }

    private void RefreshPosition(YogaFrame value) {
        YogaNode.StyleSetPosition(Edge.Top, value.top);
        YogaNode.StyleSetPosition(Edge.Bottom, value.bottom);
        YogaNode.StyleSetPosition(Edge.Left, value.left);
        YogaNode.StyleSetPosition(Edge.Right, value.right);
    }

    private void RefreshAllProperties() {
        if (_positionType.HasValue) {
            YogaNode.StyleSetPositionType(_positionType.Value);
        }
        if (_alignSelf.HasValue) {
            YogaNode.StyleSetAlignSelf(_alignSelf.Value);
        }
        if (_flexBasis.HasValue) {
            YogaNode.StyleSetFlexBasis(_flexBasis.Value);
        }
        if (_flexGrow.HasValue) {
            YogaNode.StyleSetFlexGrow(_flexGrow.Value);
        }
        if (_flexShrink.HasValue) {
            YogaNode.StyleSetFlexShrink(_flexShrink.Value);
        }
        if (_flex.HasValue) {
            YogaNode.StyleSetFlex(_flex.Value);
        }
            
        if (_position.HasValue) {
            RefreshPosition(_position.Value);
        }

        if (_size.HasValue) {
            YogaNode.StyleSetWidth(_size.Value.x);
            YogaNode.StyleSetHeight(_size.Value.y);
        }
            
        if (_minSize.HasValue) {
            YogaNode.StyleSetMinWidth(_minSize.Value.x);
            YogaNode.StyleSetMinHeight(_minSize.Value.y);
        }

        if (_maxSize.HasValue) {
            YogaNode.StyleSetMaxWidth(_maxSize.Value.x);
            YogaNode.StyleSetMaxHeight(_maxSize.Value.y);
        }

        if (_aspectRatio.HasValue) {
            YogaNode.StyleSetAspectRatio(_aspectRatio.Value);
        }

        if (_margin.HasValue) {
            RefreshMargin(_margin.Value);
        }
    }

    #endregion

    #region Context

    public override Type ContextType { get; } = typeof(YogaContext);

    public override object CreateContext() => new YogaContext();

    public override void ProvideContext(object? context) {
        if (context == null) {
            _node = null;
            return;
        }

        _node = ((YogaContext)context).YogaNode;

        // No need to update if nothing was changed
        if (_changedCacheBeforeInit) {
            RefreshAllProperties();
        }
    }

    #endregion

    #region Modifier

    internal YogaNode YogaNode {
        get {
            if (!HasValidNode) {
                throw new Exception("Node was not initialized");
            }
            return _node!;
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
            HandleLayoutItemStateUpdated(_item);

            if (_leafItem != null) {
                _leafItem.LeafLayoutUpdatedEvent += HandleLeafLayoutUpdated;

                YogaNode.SetMeasureFunc(MeasureFuncWrapper);
                HandleLeafLayoutUpdated(_leafItem);
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
        if (HasValidNode && similar.HasValidNode) {
            similar.YogaNode.CopyStyleTo(YogaNode);
            NotifyModifierUpdated();
        }
    }

    public override ILayoutModifier CreateCopy() {
        var copy = new YogaModifier {
            _positionType = _positionType,
            _alignSelf = _alignSelf,
            _flexBasis = _flexBasis,
            _flexGrow = _flexGrow,
            _flexShrink = _flexShrink,
            _position = _position,
            _size = _size,
            _minSize = _minSize,
            _maxSize = _maxSize,
            _aspectRatio = _aspectRatio,
            _margin = _margin
        };

        return copy;
    }

    #endregion
}