using System;
using System.Runtime.InteropServices;

namespace Reactive.Yoga {
    internal static class YogaNative {
        static YogaNative() {
            YogaConfig.Default.SetPointScaleFactor(0f);
            YogaConfig.Default.SetDefaultLogger();
        }

        private const string YogaDllName = "yoga";

        #region YGConfig

        [DllImport(YogaDllName)]
        public static extern IntPtr YGConfigGetDefault();

        [DllImport(YogaDllName)]
        public static extern void YGConfigSetPointScaleFactor(IntPtr ptr, float factor);

        [DllImport(YogaDllName, EntryPoint = "YGBindingsConfigSetLogger")]
        public static extern void YGConfigSetLogger(IntPtr ptr, YogaLoggerDelegate? logger);

        #endregion

        #region YGNode

        [DllImport(YogaDllName)]
        public static extern IntPtr YGNodeNew();

        [DllImport(YogaDllName)]
        public static extern void YGNodeFree(IntPtr node);

        [DllImport(YogaDllName)]
        public static extern void YGNodeInsertChild(IntPtr node, IntPtr child, int index);

        [DllImport(YogaDllName)]
        public static extern void YGNodeRemoveChild(IntPtr node, IntPtr child);

        [DllImport(YogaDllName)]
        public static extern void YGNodeRemoveAllChildren(IntPtr node);

        [DllImport(YogaDllName)]
        public static extern IntPtr YGNodeGetParent(IntPtr node);

        [DllImport(YogaDllName)]
        public static extern int YGNodeGetChildCount(IntPtr node);

        [DllImport(YogaDllName)]
        public static extern IntPtr YGNodeGetChild(IntPtr node, int index);

        #endregion

        #region YGNodeLayout

        [DllImport(YogaDllName)]
        public static extern void YGNodeSetMeasureFunc(IntPtr node, YGMeasureFunc? measureFunc);

        [DllImport(YogaDllName)]
        public static extern bool YGNodeHasMeasureFunc(IntPtr node);

        [DllImport(YogaDllName)]
        public static extern bool YGNodeGetHasNewLayout(IntPtr node);

        [DllImport(YogaDllName)]
        public static extern void YGNodeSetHasNewLayout(IntPtr node, bool value);

        [DllImport(YogaDllName)]
        public static extern bool YGNodeHadOverflow(IntPtr node);

        [DllImport(YogaDllName)]
        public static extern void YGNodeMarkDirty(IntPtr node);

        [DllImport(YogaDllName)]
        public static extern void YGNodeCalculateLayout(IntPtr node, float availableWidth, float availableHeight, Direction ownerDirection);

        [DllImport(YogaDllName)]
        public static extern float YGNodeLayoutGetLeft(IntPtr node);

        [DllImport(YogaDllName)]
        public static extern float YGNodeLayoutGetTop(IntPtr node);

        [DllImport(YogaDllName)]
        public static extern float YGNodeLayoutGetWidth(IntPtr node);

        [DllImport(YogaDllName)]
        public static extern float YGNodeLayoutGetHeight(IntPtr node);

        #endregion

        #region FlexBasis

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetFlexBasis(IntPtr node, float flexBasis);

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetFlexBasisPercent(IntPtr node, float flexBasis);

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetFlexBasisAuto(IntPtr node);

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetFlexBasisMaxContent(IntPtr node);

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetFlexBasisFitContent(IntPtr node);

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetFlexBasisStretch(IntPtr node);

        #endregion

        #region Position

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetPosition(IntPtr node, Edge edge, float position);

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetPositionPercent(IntPtr node, Edge edge, float position);

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetPositionAuto(IntPtr node, Edge edge);

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetPositionType(IntPtr node, PositionType positionType);

        #endregion

        #region Margin

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetMargin(IntPtr node, Edge edge, float margin);

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetMarginPercent(IntPtr node, Edge edge, float margin);

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetMarginAuto(IntPtr node, Edge edge);

        #endregion

        #region Padding

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetPadding(IntPtr node, Edge edge, float padding);

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetPaddingPercent(IntPtr node, Edge edge, float padding);

        #endregion

        #region Width

        [DllImport(YogaDllName)]
        public static extern YogaValue YGNodeStyleGetWidth(IntPtr node);

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetWidth(IntPtr node, float width);

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetWidthPercent(IntPtr node, float width);

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetWidthAuto(IntPtr node);

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetWidthMaxContent(IntPtr node);

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetWidthFitContent(IntPtr node);

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetWidthStretch(IntPtr node);

        #endregion

        #region Height

        [DllImport(YogaDllName)]
        public static extern YogaValue YGNodeStyleGetHeight(IntPtr node);

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetHeight(IntPtr node, float height);

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetHeightPercent(IntPtr node, float height);

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetHeightAuto(IntPtr node);

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetHeightMaxContent(IntPtr node);

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetHeightFitContent(IntPtr node);

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetHeightStretch(IntPtr node);

        #endregion

        #region MinWidth

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetMinWidth(IntPtr node, float minWidth);

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetMinWidthPercent(IntPtr node, float minWidth);

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetMinWidthMaxContent(IntPtr node);

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetMinWidthFitContent(IntPtr node);

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetMinWidthStretch(IntPtr node);

        #endregion

        #region MinHeight

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetMinHeight(IntPtr node, float minHeight);

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetMinHeightPercent(IntPtr node, float minHeight);

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetMinHeightMaxContent(IntPtr node);

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetMinHeightFitContent(IntPtr node);

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetMinHeightStretch(IntPtr node);

        #endregion

        #region MaxWidth

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetMaxWidth(IntPtr node, float maxWidth);

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetMaxWidthPercent(IntPtr node, float maxWidth);

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetMaxWidthMaxContent(IntPtr node);

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetMaxWidthFitContent(IntPtr node);

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetMaxWidthStretch(IntPtr node);

        #endregion

        #region MaxHeight

        [DllImport(YogaDllName, EntryPoint = "YGNodeStyleSetMaxHeight")]
        public static extern void YGNodeStyleSetMaxHeight(IntPtr node, float maxHeight);

        [DllImport(YogaDllName, EntryPoint = "YGNodeStyleSetMaxHeightPercent")]
        public static extern void YGNodeStyleSetMaxHeightPercent(IntPtr node, float maxHeight);

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetMaxHeightMaxContent(IntPtr node);

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetMaxHeightFitContent(IntPtr node);

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetMaxHeightStretch(IntPtr node);

        #endregion

        #region Gap

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetGap(IntPtr node, Gutter gutter, float gap);

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetGapPercent(IntPtr node, Gutter gutter, float gap);

        #endregion

        #region Other Style Properties

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetOverflow(IntPtr node, Overflow overflow);

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetAspectRatio(IntPtr node, float aspectRatio);

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetDisplay(IntPtr node, Display display);

        [DllImport(YogaDllName)]
        public static extern Display YGNodeStyleGetDisplay(IntPtr node);

        [DllImport(YogaDllName)]
        public static extern void YGNodeCopyStyle(IntPtr destination, IntPtr source);

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetDirection(IntPtr node, Direction direction);

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetFlexDirection(IntPtr node, FlexDirection flexDirection);

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetJustifyContent(IntPtr node, Justify justifyContent);

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetAlignContent(IntPtr node, Align alignContent);

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetAlignItems(IntPtr node, Align alignItems);

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetAlignSelf(IntPtr node, Align alignSelf);

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetFlexWrap(IntPtr node, Wrap flexWrap);

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetFlex(IntPtr node, float flex);

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetFlexGrow(IntPtr node, float flexGrow);

        [DllImport(YogaDllName)]
        public static extern void YGNodeStyleSetFlexShrink(IntPtr node, float flexShrink);

        #endregion
    }
}