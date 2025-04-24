using System;
using System.Collections.Generic;
using UnityEngine;

namespace Reactive.Yoga {
    internal class YogaNode : IDisposable, IEquatable<YogaNode> {
        #region Factory

        private YogaNode() { }

        private static readonly Dictionary<IntPtr, YogaNode> managedNodes = new();

        public static YogaNode New() {
            var node = new YogaNode {
                _nodePtr = YogaNative.YGNodeNew()
            };

            managedNodes.Add(node._nodePtr, node);

            return node;
        }

        public static YogaNode? GetManaged(IntPtr ptr) {
            if (ptr == IntPtr.Zero) {
                return null;
            }

            if (!managedNodes.TryGetValue(ptr, out var node)) {
                throw WrapperMissing(ptr);
            }

            return node;
        }

        public static YogaNode GetManagedOrThrow(IntPtr ptr) {
            return GetManaged(ptr) ?? throw WrapperMissing(ptr);
        }

        private static Exception WrapperMissing(IntPtr ptr) {
            return new InvalidOperationException($"Managed wrapper for pointer {ptr.ToString()} was not presented");
        }

        #endregion

        #region State

        public bool IsInitialized => _nodePtr != IntPtr.Zero;

        private IntPtr NodePtr {
            get {
                if (_nodePtr == IntPtr.Zero) {
                    throw new InvalidOperationException("YogaNode was not initialized");
                }

                return _nodePtr;
            }
        }

        private IntPtr _nodePtr;

        public void Dispose() {
            if (NodePtr == IntPtr.Zero) {
                return;
            }

            YogaNative.YGNodeFree(NodePtr);

            managedNodes.Remove(_nodePtr);
            _nodePtr = IntPtr.Zero;
        }

        public bool Equals(YogaNode other) {
            return NodePtr == other.NodePtr;
        }

        public override string ToString() {
            return $"calc: {{ w: {LayoutGetWidth()}, h: {LayoutGetHeight()} }}, set: {{ w: {StyleGetWidth()}, h: {StyleGetHeight()}, dsp: {StyleGetDisplay()} }}";
        }

        #endregion

        #region Automatic Style Setters

        public void StyleSetPosition(Edge edge, YogaValue value) {
            switch (value.unit) {
                case Unit.Undefined:
                    YogaNative.YGNodeStyleSetPosition(NodePtr, edge, float.NaN);
                    break;

                case Unit.Point:
                    YogaNative.YGNodeStyleSetPosition(NodePtr, edge, value.value);
                    break;

                case Unit.Percent:
                    YogaNative.YGNodeStyleSetPositionPercent(NodePtr, edge, value.value);
                    break;

                case Unit.Auto:
                    YogaNative.YGNodeStyleSetPositionAuto(NodePtr, edge);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(value), $"{value.unit} is not supported for position");
            }
        }

        public void StyleSetWidth(YogaValue value) {
            switch (value.unit) {
                case Unit.Undefined:
                    YogaNative.YGNodeStyleSetWidth(NodePtr, float.NaN);
                    break;

                case Unit.Point:
                    YogaNative.YGNodeStyleSetWidth(NodePtr, value.value);
                    break;

                case Unit.Percent:
                    YogaNative.YGNodeStyleSetWidthPercent(NodePtr, value.value);
                    break;

                case Unit.Auto:
                    YogaNative.YGNodeStyleSetWidthAuto(NodePtr);
                    break;

                case Unit.MaxContent:
                    YogaNative.YGNodeStyleSetWidthMaxContent(NodePtr);
                    break;

                case Unit.FitContent:
                    YogaNative.YGNodeStyleSetWidthFitContent(NodePtr);
                    break;

                case Unit.Stretch:
                    YogaNative.YGNodeStyleSetWidthStretch(NodePtr);
                    break;
            }
        }

        public void StyleSetHeight(YogaValue value) {
            switch (value.unit) {
                case Unit.Undefined:
                    YogaNative.YGNodeStyleSetHeight(NodePtr, float.NaN);
                    break;

                case Unit.Point:
                    YogaNative.YGNodeStyleSetHeight(NodePtr, value.value);
                    break;

                case Unit.Percent:
                    YogaNative.YGNodeStyleSetHeightPercent(NodePtr, value.value);
                    break;

                case Unit.Auto:
                    YogaNative.YGNodeStyleSetHeightAuto(NodePtr);
                    break;

                case Unit.MaxContent:
                    YogaNative.YGNodeStyleSetHeightMaxContent(NodePtr);
                    break;

                case Unit.FitContent:
                    YogaNative.YGNodeStyleSetHeightFitContent(NodePtr);
                    break;

                case Unit.Stretch:
                    YogaNative.YGNodeStyleSetHeightStretch(NodePtr);
                    break;
            }
        }

        public void StyleSetMinWidth(YogaValue value) {
            switch (value.unit) {
                case Unit.Undefined:
                    YogaNative.YGNodeStyleSetMinWidth(NodePtr, float.NaN);
                    break;

                case Unit.Point:
                    YogaNative.YGNodeStyleSetMinWidth(NodePtr, value.value);
                    break;

                case Unit.Percent:
                    YogaNative.YGNodeStyleSetMinWidthPercent(NodePtr, value.value);
                    break;

                case Unit.MaxContent:
                    YogaNative.YGNodeStyleSetMinWidthMaxContent(NodePtr);
                    break;

                case Unit.FitContent:
                    YogaNative.YGNodeStyleSetMinWidthFitContent(NodePtr);
                    break;

                case Unit.Stretch:
                    YogaNative.YGNodeStyleSetMinWidthStretch(NodePtr);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(value), $"{value.unit} is not supported for min width");
            }
        }

        public void StyleSetMaxWidth(YogaValue value) {
            switch (value.unit) {
                case Unit.Undefined:
                    YogaNative.YGNodeStyleSetMaxWidth(NodePtr, float.NaN);
                    break;

                case Unit.Point:
                    YogaNative.YGNodeStyleSetMaxWidth(NodePtr, value.value);
                    break;

                case Unit.Percent:
                    YogaNative.YGNodeStyleSetMaxWidthPercent(NodePtr, value.value);
                    break;

                case Unit.MaxContent:
                    YogaNative.YGNodeStyleSetMaxWidthMaxContent(NodePtr);
                    break;

                case Unit.FitContent:
                    YogaNative.YGNodeStyleSetMaxWidthFitContent(NodePtr);
                    break;

                case Unit.Stretch:
                    YogaNative.YGNodeStyleSetMaxWidthStretch(NodePtr);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(value), $"{value.unit} is not supported for max width");
            }
        }

        public void StyleSetMinHeight(YogaValue value) {
            switch (value.unit) {
                case Unit.Undefined:
                    YogaNative.YGNodeStyleSetMinHeight(NodePtr, float.NaN);
                    break;

                case Unit.Point:
                    YogaNative.YGNodeStyleSetMinHeight(NodePtr, value.value);
                    break;

                case Unit.Percent:
                    YogaNative.YGNodeStyleSetMinHeightPercent(NodePtr, value.value);
                    break;

                case Unit.MaxContent:
                    YogaNative.YGNodeStyleSetMinHeightMaxContent(NodePtr);
                    break;

                case Unit.FitContent:
                    YogaNative.YGNodeStyleSetMinHeightFitContent(NodePtr);
                    break;

                case Unit.Stretch:
                    YogaNative.YGNodeStyleSetMinHeightStretch(NodePtr);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(value), $"{value.unit} is not supported for min height");
            }
        }

        public void StyleSetMaxHeight(YogaValue value) {
            switch (value.unit) {
                case Unit.Undefined:
                    YogaNative.YGNodeStyleSetMaxHeight(NodePtr, float.NaN);
                    break;

                case Unit.Point:
                    YogaNative.YGNodeStyleSetMaxHeight(NodePtr, value.value);
                    break;

                case Unit.Percent:
                    YogaNative.YGNodeStyleSetMaxHeightPercent(NodePtr, value.value);
                    break;

                case Unit.MaxContent:
                    YogaNative.YGNodeStyleSetMaxHeightMaxContent(NodePtr);
                    break;

                case Unit.FitContent:
                    YogaNative.YGNodeStyleSetMaxHeightFitContent(NodePtr);
                    break;

                case Unit.Stretch:
                    YogaNative.YGNodeStyleSetMaxHeightStretch(NodePtr);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(value), $"{value.unit} is not supported for max height");
            }
        }

        public void StyleSetFlexBasis(YogaValue value) {
            switch (value.unit) {
                case Unit.Undefined:
                    YogaNative.YGNodeStyleSetFlexBasis(NodePtr, float.NaN);
                    break;

                case Unit.Point:
                    YogaNative.YGNodeStyleSetFlexBasis(NodePtr, value.value);
                    break;

                case Unit.Percent:
                    YogaNative.YGNodeStyleSetFlexBasisPercent(NodePtr, value.value);
                    break;

                case Unit.Auto:
                    YogaNative.YGNodeStyleSetFlexBasisAuto(NodePtr);
                    break;

                case Unit.MaxContent:
                    YogaNative.YGNodeStyleSetFlexBasisMaxContent(NodePtr);
                    break;

                case Unit.FitContent:
                    YogaNative.YGNodeStyleSetFlexBasisFitContent(NodePtr);
                    break;

                case Unit.Stretch:
                    YogaNative.YGNodeStyleSetFlexBasisStretch(NodePtr);
                    break;
            }
        }

        public void StyleSetMargin(Edge edge, YogaValue value) {
            switch (value.unit) {
                case Unit.Undefined:
                    YogaNative.YGNodeStyleSetMargin(NodePtr, edge, float.NaN);
                    break;

                case Unit.Point:
                    YogaNative.YGNodeStyleSetMargin(NodePtr, edge, value.value);
                    break;

                case Unit.Percent:
                    YogaNative.YGNodeStyleSetMarginPercent(NodePtr, edge, value.value);
                    break;

                case Unit.Auto:
                    YogaNative.YGNodeStyleSetMarginAuto(NodePtr, edge);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(value), $"{value.unit} is not supported for padding");
            }
        }

        public void StyleSetPadding(Edge edge, YogaValue value) {
            switch (value.unit) {
                case Unit.Undefined:
                    YogaNative.YGNodeStyleSetPadding(NodePtr, edge, float.NaN);
                    break;

                case Unit.Point:
                    YogaNative.YGNodeStyleSetPadding(NodePtr, edge, value.value);
                    break;

                case Unit.Percent:
                    YogaNative.YGNodeStyleSetPaddingPercent(NodePtr, edge, value.value);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(value), $"{value.unit} is not supported for padding");
            }
        }

        public void StyleSetGap(Gutter gutter, YogaValue value) {
            switch (value.unit) {
                case Unit.Undefined:
                    YogaNative.YGNodeStyleSetGap(NodePtr, gutter, float.NaN);
                    break;

                case Unit.Point:
                    YogaNative.YGNodeStyleSetGap(NodePtr, gutter, value.value);
                    break;

                case Unit.Percent:
                    YogaNative.YGNodeStyleSetGapPercent(NodePtr, gutter, value.value);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(value), $"{value.unit} is not supported for gap");
            }
        }

        #endregion

        #region Children

        // Just for easier debugging
        public IEnumerable<YogaNode> Children {
            get {
                var count = GetChildCount();

                for (var i = 0; i < count; i++) {
                    yield return GetChild(i);
                }
            }
        }

        public YogaNode? GetParent() {
            var ptr = YogaNative.YGNodeGetParent(NodePtr);

            return GetManaged(ptr);
        }

        public void InsertChild(YogaNode child, int index) {
            if (index < 0 || index > GetChildCount()) {
                throw new ArgumentOutOfRangeException(nameof(child), "Index must be >= zero and <= child count");
            }

            YogaNative.YGNodeInsertChild(NodePtr, child.NodePtr, index);
        }

        public int GetChildCount() {
            return YogaNative.YGNodeGetChildCount(NodePtr);
        }

        public YogaNode GetChild(int index) {
            var ptr = YogaNative.YGNodeGetChild(NodePtr, index);

            return GetManagedOrThrow(ptr);
        }

        public void RemoveChild(YogaNode child) {
            YogaNative.YGNodeRemoveChild(NodePtr, child.NodePtr);
        }

        public void RemoveAllChildren() {
            YogaNative.YGNodeRemoveAllChildren(NodePtr);
        }

        #endregion

        #region Layout

        // This field is crucial as runtime does not track references passed to the native code, so GC can
        // easily collect the delegate. This would cause a crash when native code attempts to call the delegate
        private YGMeasureFunc? _measureFunc;

        public void SetMeasureFunc(YGMeasureFunc? measureFunc) {
            _measureFunc = measureFunc;
            YogaNative.YGNodeSetMeasureFunc(NodePtr, measureFunc);
        }

        public bool HasMeasureFunc() {
            return YogaNative.YGNodeHasMeasureFunc(NodePtr);
        }

        public bool HadOverflow() {
            return YogaNative.YGNodeGetHadOverflow(NodePtr);
        }

        public float LayoutGetLeft() {
            return YogaNative.YGNodeLayoutGetLeft(NodePtr);
        }

        public float LayoutGetTop() {
            return YogaNative.YGNodeLayoutGetTop(NodePtr);
        }

        public float LayoutGetWidth() {
            return YogaNative.YGNodeLayoutGetWidth(NodePtr);
        }

        public float LayoutGetHeight() {
            return YogaNative.YGNodeLayoutGetHeight(NodePtr);
        }

        public void ApplyTo(RectTransform rectTransform) {
            rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, LayoutGetTop(), LayoutGetHeight());
            rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, LayoutGetLeft(), LayoutGetWidth());
        }

        public void ApplySizeTo(RectTransform rectTransform) {
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, LayoutGetWidth());
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, LayoutGetHeight());
        }

        public void CalculateLayout(float availableWidth, float availableHeight, Direction ownerDirection) {
            // NaN values allow the node to determine it's size without constraints
            //if (StyleGetWidth() == YogaValue.Undefined) {
            //    availableWidth = float.NaN;
            //}

            //if (StyleGetHeight() == YogaValue.Undefined) {
            //    availableHeight = float.NaN;
            //}

            YogaNative.YGNodeCalculateLayout(NodePtr, availableWidth, availableHeight, ownerDirection);
        }

        public bool GetHasNewLayout() {
            return YogaNative.YGNodeGetHasNewLayout(NodePtr);
        }

        public void SetHasNewLayout(bool hasNewLayout) {
            YogaNative.YGNodeSetHasNewLayout(NodePtr, hasNewLayout);
        }

        public void MarkDirty() {
            YogaNative.YGNodeMarkDirty(NodePtr);
        }

        #endregion

        #region Style Setters

        public void CopyStyleTo(YogaNode node) {
            YogaNative.YGNodeCopyStyle(node.NodePtr, NodePtr);
        }

        public void StyleSetFlex(float flex) {
            YogaNative.YGNodeStyleSetFlex(NodePtr, flex);
        }

        public void StyleSetAspectRatio(float aspectRatio) {
            YogaNative.YGNodeStyleSetAspectRatio(NodePtr, aspectRatio);
        }

        public void StyleSetOverflow(Overflow overflow) {
            YogaNative.YGNodeStyleSetOverflow(NodePtr, overflow);
        }

        public void StyleSetDirection(Direction direction) {
            YogaNative.YGNodeStyleSetDirection(NodePtr, direction);
        }

        public void StyleSetFlexDirection(FlexDirection flexDirection) {
            YogaNative.YGNodeStyleSetFlexDirection(NodePtr, flexDirection);
        }

        public void StyleSetJustifyContent(Justify justifyContent) {
            YogaNative.YGNodeStyleSetJustifyContent(NodePtr, justifyContent);
        }

        public void StyleSetAlignContent(Align alignContent) {
            YogaNative.YGNodeStyleSetAlignContent(NodePtr, alignContent);
        }

        public void StyleSetAlignItems(Align alignItems) {
            YogaNative.YGNodeStyleSetAlignItems(NodePtr, alignItems);
        }

        public void StyleSetAlignSelf(Align alignSelf) {
            YogaNative.YGNodeStyleSetAlignSelf(NodePtr, alignSelf);
        }

        public void StyleSetPositionType(PositionType positionType) {
            YogaNative.YGNodeStyleSetPositionType(NodePtr, positionType);
        }

        public void StyleSetFlexWrap(Wrap flexWrap) {
            YogaNative.YGNodeStyleSetFlexWrap(NodePtr, flexWrap);
        }

        public void StyleSetFlexGrow(float flexGrow) {
            YogaNative.YGNodeStyleSetFlexGrow(NodePtr, flexGrow);
        }

        public void StyleSetFlexShrink(float flexShrink) {
            YogaNative.YGNodeStyleSetFlexShrink(NodePtr, flexShrink);
        }

        public void StyleSetDisplay(Display display) {
            YogaNative.YGNodeStyleSetDisplay(NodePtr, display);
        }

        #endregion

        #region Style Getters

        public float StyleGetFlex() {
            return YogaNative.YGNodeStyleGetFlex(NodePtr);
        }

        public YogaValue StyleGetHeight() {
            return YogaNative.YGNodeStyleGetHeight(NodePtr);
        }

        public YogaValue StyleGetWidth() {
            return YogaNative.YGNodeStyleGetWidth(NodePtr);
        }

        public YogaValue StyleGetMinWidth() {
            return YogaNative.YGNodeStyleGetMinWidth(NodePtr);
        }

        public YogaValue StyleGetMinHeight() {
            return YogaNative.YGNodeStyleGetMinHeight(NodePtr);
        }

        public YogaValue StyleGetMaxWidth() {
            return YogaNative.YGNodeStyleGetMaxWidth(NodePtr);
        }

        public YogaValue StyleGetMaxHeight() {
            return YogaNative.YGNodeStyleGetMaxHeight(NodePtr);
        }

        public float StyleGetAspectRatio() {
            return YogaNative.YGNodeStyleGetAspectRatio(NodePtr);
        }

        public Display StyleGetDisplay() {
            return YogaNative.YGNodeStyleGetDisplay(NodePtr);
        }

        public YogaValue StyleGetPosition(Edge edge) {
            return YogaNative.YGNodeStyleGetPosition(NodePtr, edge);
        }

        public YogaValue StyleGetMargin(Edge edge) {
            return YogaNative.YGNodeStyleGetMargin(NodePtr, edge);
        }

        public YogaValue StyleGetPadding(Edge edge) {
            return YogaNative.YGNodeStyleGetPadding(NodePtr, edge);
        }

        public float StyleGetGap(Gutter gutter) {
            return YogaNative.YGNodeStyleGetGap(NodePtr, gutter);
        }

        public PositionType StyleGetPositionType() {
            return YogaNative.YGNodeStyleGetPositionType(NodePtr);
        }

        public float StyleGetFlexGrow() {
            return YogaNative.YGNodeStyleGetFlexGrow(NodePtr);
        }

        public float StyleGetFlexShrink() {
            return YogaNative.YGNodeStyleGetFlexShrink(NodePtr);
        }

        public YogaValue StyleGetFlexBasis() {
            return YogaNative.YGNodeStyleGetFlexBasis(NodePtr);
        }

        public Overflow StyleGetOverflow() {
            return YogaNative.YGNodeStyleGetOverflow(NodePtr);
        }

        public Direction StyleGetDirection() {
            return YogaNative.YGNodeStyleGetDirection(NodePtr);
        }

        public FlexDirection StyleGetFlexDirection() {
            return YogaNative.YGNodeStyleGetFlexDirection(NodePtr);
        }

        public Justify StyleGetJustifyContent() {
            return YogaNative.YGNodeStyleGetJustifyContent(NodePtr);
        }

        public Align StyleGetAlignContent() {
            return YogaNative.YGNodeStyleGetAlignContent(NodePtr);
        }

        public Align StyleGetAlignItems() {
            return YogaNative.YGNodeStyleGetAlignItems(NodePtr);
        }

        public Align StyleGetAlignSelf() {
            return YogaNative.YGNodeStyleGetAlignSelf(NodePtr);
        }

        public Wrap StyleGetFlexWrap() {
            return YogaNative.YGNodeStyleGetFlexWrap(NodePtr);
        }

        #endregion
    }
}