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
                    StyleSetPosition(edge, float.NaN);
                    break;

                case Unit.Point:
                    StyleSetPosition(edge, value.value);
                    break;

                case Unit.Percent:
                    StyleSetPositionPercent(edge, value.value);
                    break;

                case Unit.Auto:
                    throw new ArgumentOutOfRangeException(nameof(value), "Auto is not supported for position");
            }
        }

        public void StyleSetWidth(YogaValue value) {
            switch (value.unit) {
                case Unit.Undefined:
                    StyleSetWidth(float.NaN);
                    break;

                case Unit.Point:
                    StyleSetWidth(value.value);
                    break;

                case Unit.Percent:
                    StyleSetWidthPercent(value.value);
                    break;

                case Unit.Auto:
                    StyleSetWidthAuto();
                    break;
            }
        }

        public void StyleSetHeight(YogaValue value) {
            switch (value.unit) {
                case Unit.Undefined:
                    StyleSetHeight(float.NaN);
                    break;

                case Unit.Point:
                    StyleSetHeight(value.value);
                    break;

                case Unit.Percent:
                    StyleSetHeightPercent(value.value);
                    break;

                case Unit.Auto:
                    StyleSetHeightAuto();
                    break;
            }
        }

        public void StyleSetMinWidth(YogaValue value) {
            switch (value.unit) {
                case Unit.Undefined:
                    StyleSetMinWidth(float.NaN);
                    break;

                case Unit.Point:
                    StyleSetMinWidth(value.value);
                    break;

                case Unit.Percent:
                    StyleSetMinWidthPercent(value.value);
                    break;

                case Unit.Auto:
                    throw new ArgumentOutOfRangeException(nameof(value), "Auto is not supported for min width");
            }
        }

        public void StyleSetMaxWidth(YogaValue value) {
            switch (value.unit) {
                case Unit.Undefined:
                    StyleSetMaxWidth(float.NaN);
                    break;

                case Unit.Point:
                    StyleSetMaxWidth(value.value);
                    break;

                case Unit.Percent:
                    StyleSetMaxWidthPercent(value.value);
                    break;

                case Unit.Auto:
                    throw new ArgumentOutOfRangeException(nameof(value), "Auto is not supported for max width");
            }
        }

        public void StyleSetMinHeight(YogaValue value) {
            switch (value.unit) {
                case Unit.Undefined:
                    StyleSetMinHeight(float.NaN);
                    break;

                case Unit.Point:
                    StyleSetMinHeight(value.value);
                    break;

                case Unit.Percent:
                    StyleSetMinHeightPercent(value.value);
                    break;

                case Unit.Auto:
                    throw new ArgumentOutOfRangeException(nameof(value), "Auto is not supported for min height");
            }
        }

        public void StyleSetMaxHeight(YogaValue value) {
            switch (value.unit) {
                case Unit.Undefined:
                    StyleSetMaxHeight(float.NaN);
                    break;

                case Unit.Point:
                    StyleSetMaxHeight(value.value);
                    break;

                case Unit.Percent:
                    StyleSetMaxHeightPercent(value.value);
                    break;

                case Unit.Auto:
                    throw new ArgumentOutOfRangeException(nameof(value), "Auto is not supported for max height");
            }
        }

        public void StyleSetFlexBasis(YogaValue value) {
            switch (value.unit) {
                case Unit.Undefined:
                    StyleSetFlexBasis(float.NaN);
                    break;

                case Unit.Point:
                    StyleSetFlexBasis(value.value);
                    break;

                case Unit.Percent:
                    StyleSetFlexBasisPercent(value.value);
                    break;

                case Unit.Auto:
                    StyleSetFlexBasisAuto();
                    break;
            }
        }

        public void StyleSetMargin(Edge edge, YogaValue value) {
            switch (value.unit) {
                case Unit.Undefined:
                    StyleSetMargin(edge, float.NaN);
                    break;

                case Unit.Point:
                    StyleSetMargin(edge, value.value);
                    break;

                case Unit.Percent:
                    StyleSetMarginPercent(edge, value.value);
                    break;

                case Unit.Auto:
                    StyleSetMarginAuto(edge);
                    break;
            }
        }

        public void StyleSetPadding(Edge edge, YogaValue value) {
            switch (value.unit) {
                case Unit.Undefined:
                    StyleSetPadding(edge, float.NaN);
                    break;

                case Unit.Point:
                    StyleSetPadding(edge, value.value);
                    break;

                case Unit.Percent:
                    StyleSetPaddingPercent(edge, value.value);
                    break;

                case Unit.Auto:
                    throw new ArgumentOutOfRangeException(nameof(value), "Auto is not supported for padding");
            }
        }

        public void StyleSetGap(Gutter gutter, YogaValue value) {
            switch (value.unit) {
                case Unit.Undefined:
                    StyleSetGap(gutter, float.NaN);
                    break;

                case Unit.Point:
                    StyleSetGap(gutter, value.value);
                    break;

                case Unit.Percent:
                    StyleSetGapPercent(gutter, value.value);
                    break;

                case Unit.Auto:
                    throw new ArgumentOutOfRangeException(nameof(value), "Auto is not supported for gap");
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
            YogaNative.YGNodeCalculateLayout(NodePtr, availableWidth, availableHeight, ownerDirection);
        }

        public bool GetHasNewLayout() {
            return YogaNative.YGNodeGetHasNewLayout(NodePtr);
        }

        public void SetHasNewLayout(bool hasNewLayout) {
            YogaNative.YGNodeSetHasNewLayout(NodePtr, hasNewLayout);
        }

        #endregion

        #region Style

        public YogaValue StyleGetHeight() {
            return YogaNative.YGNodeStyleGetHeight(NodePtr);
        }

        public YogaValue StyleGetWidth() {
            return YogaNative.YGNodeStyleGetWidth(NodePtr);
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

        public void StyleSetFlexBasis(float flexBasis) {
            YogaNative.YGNodeStyleSetFlexBasis(NodePtr, flexBasis);
        }

        public void StyleSetFlexBasisPercent(float flexBasis) {
            YogaNative.YGNodeStyleSetFlexBasisPercent(NodePtr, flexBasis);
        }

        public void StyleSetFlexBasisAuto() {
            YogaNative.YGNodeStyleSetFlexBasisAuto(NodePtr);
        }

        public void StyleSetPosition(Edge edge, float position) {
            YogaNative.YGNodeStyleSetPosition(NodePtr, edge, position);
        }

        public void StyleSetPositionPercent(Edge edge, float position) {
            YogaNative.YGNodeStyleSetPositionPercent(NodePtr, edge, position);
        }

        public void StyleSetMargin(Edge edge, float margin) {
            YogaNative.YGNodeStyleSetMargin(NodePtr, edge, margin);
        }

        public void StyleSetMarginPercent(Edge edge, float margin) {
            YogaNative.YGNodeStyleSetMarginPercent(NodePtr, edge, margin);
        }

        public void StyleSetMarginAuto(Edge edge) {
            YogaNative.YGNodeStyleSetMarginAuto(NodePtr, edge);
        }

        public void StyleSetPadding(Edge edge, float padding) {
            YogaNative.YGNodeStyleSetPadding(NodePtr, edge, padding);
        }

        public void StyleSetPaddingPercent(Edge edge, float padding) {
            YogaNative.YGNodeStyleSetPaddingPercent(NodePtr, edge, padding);
        }

        public void StyleSetWidth(float width) {
            YogaNative.YGNodeStyleSetWidth(NodePtr, width);
        }

        public void StyleSetWidthPercent(float width) {
            YogaNative.YGNodeStyleSetWidthPercent(NodePtr, width);
        }

        public void StyleSetWidthAuto() {
            YogaNative.YGNodeStyleSetWidthAuto(NodePtr);
        }

        public void StyleSetHeight(float height) {
            YogaNative.YGNodeStyleSetHeight(NodePtr, height);
        }

        public void StyleSetHeightPercent(float height) {
            YogaNative.YGNodeStyleSetHeightPercent(NodePtr, height);
        }

        public void StyleSetHeightAuto() {
            YogaNative.YGNodeStyleSetHeightAuto(NodePtr);
        }

        public void StyleSetMinWidth(float minWidth) {
            YogaNative.YGNodeStyleSetMinWidth(NodePtr, minWidth);
        }

        public void StyleSetMinWidthPercent(float minWidth) {
            YogaNative.YGNodeStyleSetMinWidthPercent(NodePtr, minWidth);
        }

        public void StyleSetMinHeight(float minHeight) {
            YogaNative.YGNodeStyleSetMinHeight(NodePtr, minHeight);
        }

        public void StyleSetMinHeightPercent(float minHeight) {
            YogaNative.YGNodeStyleSetMinHeightPercent(NodePtr, minHeight);
        }

        public void StyleSetMaxWidth(float maxWidth) {
            YogaNative.YGNodeStyleSetMaxWidth(NodePtr, maxWidth);
        }

        public void StyleSetMaxWidthPercent(float maxWidth) {
            YogaNative.YGNodeStyleSetMaxWidthPercent(NodePtr, maxWidth);
        }

        public void StyleSetMaxHeight(float maxHeight) {
            YogaNative.YGNodeStyleSetMaxHeight(NodePtr, maxHeight);
        }

        public void StyleSetMaxHeightPercent(float maxHeight) {
            YogaNative.YGNodeStyleSetMaxHeightPercent(NodePtr, maxHeight);
        }

        public void StyleSetGap(Gutter gutter, float gap) {
            YogaNative.YGNodeStyleSetGap(NodePtr, gutter, gap);
        }

        public void StyleSetGapPercent(Gutter gutter, float gap) {
            YogaNative.YGNodeStyleSetGapPercent(NodePtr, gutter, gap);
        }

        public void StyleSetAspectRatio(float aspectRatio) {
            YogaNative.YGNodeStyleSetAspectRatio(NodePtr, aspectRatio);
        }

        public void StyleSetDisplay(Display display) {
            YogaNative.YGNodeStyleSetDisplay(NodePtr, display);
        }

        public Display StyleGetDisplay() {
            return YogaNative.YGNodeStyleGetDisplay(NodePtr);
        }

        #endregion
    }
}