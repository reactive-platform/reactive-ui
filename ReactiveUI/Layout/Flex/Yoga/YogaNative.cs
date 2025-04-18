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
        public static extern void YGNodeCalculateLayout(IntPtr node, float availableWidth, float availableHeight, Direction ownerDirection);

        [DllImport(YogaDllName, EntryPoint = "YGNodeLayoutGetLeft")]
        public static extern float YGNodeLayoutGetLeft(IntPtr node);

        [DllImport(YogaDllName, EntryPoint = "YGNodeLayoutGetTop")]
        public static extern float YGNodeLayoutGetTop(IntPtr node);

        [DllImport(YogaDllName, EntryPoint = "YGNodeLayoutGetWidth")]
        public static extern float YGNodeLayoutGetWidth(IntPtr node);

        [DllImport(YogaDllName, EntryPoint = "YGNodeLayoutGetHeight")]
        public static extern float YGNodeLayoutGetHeight(IntPtr node);

        #endregion

        #region YGNodeStyle

        [DllImport(YogaDllName, EntryPoint = "YGNodeStyleSetOverflow")]
        public static extern void YGNodeStyleSetOverflow(IntPtr node, Overflow overflow);

        [DllImport(YogaDllName, EntryPoint = "YGNodeStyleSetDirection")]
        public static extern void YGNodeStyleSetDirection(IntPtr node, Direction direction);

        [DllImport(YogaDllName, EntryPoint = "YGNodeStyleSetFlexDirection")]
        public static extern void YGNodeStyleSetFlexDirection(IntPtr node, FlexDirection flexDirection);

        [DllImport(YogaDllName, EntryPoint = "YGNodeStyleSetJustifyContent")]
        public static extern void YGNodeStyleSetJustifyContent(IntPtr node, Justify justifyContent);

        [DllImport(YogaDllName, EntryPoint = "YGNodeStyleSetAlignContent")]
        public static extern void YGNodeStyleSetAlignContent(IntPtr node, Align alignContent);

        [DllImport(YogaDllName, EntryPoint = "YGNodeStyleSetAlignItems")]
        public static extern void YGNodeStyleSetAlignItems(IntPtr node, Align alignItems);

        [DllImport(YogaDllName, EntryPoint = "YGNodeStyleSetAlignSelf")]
        public static extern void YGNodeStyleSetAlignSelf(IntPtr node, Align alignSelf);

        [DllImport(YogaDllName, EntryPoint = "YGNodeStyleSetPositionType")]
        public static extern void YGNodeStyleSetPositionType(IntPtr node, PositionType positionType);

        [DllImport(YogaDllName, EntryPoint = "YGNodeStyleSetFlexWrap")]
        public static extern void YGNodeStyleSetFlexWrap(IntPtr node, Wrap flexWrap);

        [DllImport(YogaDllName, EntryPoint = "YGNodeStyleSetFlexGrow")]
        public static extern void YGNodeStyleSetFlexGrow(IntPtr node, float flexGrow);

        [DllImport(YogaDllName, EntryPoint = "YGNodeStyleSetFlexShrink")]
        public static extern void YGNodeStyleSetFlexShrink(IntPtr node, float flexShrink);

        [DllImport(YogaDllName, EntryPoint = "YGNodeStyleSetFlexBasis")]
        public static extern void YGNodeStyleSetFlexBasis(IntPtr node, float flexBasis);

        [DllImport(YogaDllName, EntryPoint = "YGNodeStyleSetFlexBasisPercent")]
        public static extern void YGNodeStyleSetFlexBasisPercent(IntPtr node, float flexBasis);

        [DllImport(YogaDllName, EntryPoint = "YGNodeStyleSetFlexBasisAuto")]
        public static extern void YGNodeStyleSetFlexBasisAuto(IntPtr node);

        [DllImport(YogaDllName, EntryPoint = "YGNodeStyleSetPosition")]
        public static extern void YGNodeStyleSetPosition(IntPtr node, Edge edge, float position);

        [DllImport(YogaDllName, EntryPoint = "YGNodeStyleSetPositionPercent")]
        public static extern void YGNodeStyleSetPositionPercent(IntPtr node, Edge edge, float position);

        [DllImport(YogaDllName, EntryPoint = "YGNodeStyleSetMargin")]
        public static extern void YGNodeStyleSetMargin(IntPtr node, Edge edge, float margin);

        [DllImport(YogaDllName, EntryPoint = "YGNodeStyleSetMarginPercent")]
        public static extern void YGNodeStyleSetMarginPercent(IntPtr node, Edge edge, float margin);

        [DllImport(YogaDllName, EntryPoint = "YGNodeStyleSetMarginAuto")]
        public static extern void YGNodeStyleSetMarginAuto(IntPtr node, Edge edge);

        [DllImport(YogaDllName, EntryPoint = "YGNodeStyleSetPadding")]
        public static extern void YGNodeStyleSetPadding(IntPtr node, Edge edge, float padding);

        [DllImport(YogaDllName, EntryPoint = "YGNodeStyleSetPaddingPercent")]
        public static extern void YGNodeStyleSetPaddingPercent(IntPtr node, Edge edge, float padding);

        [DllImport(YogaDllName, EntryPoint = "YGNodeStyleGetWidth")]
        public static extern YogaValue YGNodeStyleGetWidth(IntPtr node);

        [DllImport(YogaDllName, EntryPoint = "YGNodeStyleSetWidth")]
        public static extern void YGNodeStyleSetWidth(IntPtr node, float width);

        [DllImport(YogaDllName, EntryPoint = "YGNodeStyleSetWidthPercent")]
        public static extern void YGNodeStyleSetWidthPercent(IntPtr node, float width);

        [DllImport(YogaDllName, EntryPoint = "YGNodeStyleSetWidthAuto")]
        public static extern void YGNodeStyleSetWidthAuto(IntPtr node);

        [DllImport(YogaDllName, EntryPoint = "YGNodeStyleGetHeight")]
        public static extern YogaValue YGNodeStyleGetHeight(IntPtr node);

        [DllImport(YogaDllName, EntryPoint = "YGNodeStyleSetHeight")]
        public static extern void YGNodeStyleSetHeight(IntPtr node, float height);

        [DllImport(YogaDllName, EntryPoint = "YGNodeStyleSetHeightPercent")]
        public static extern void YGNodeStyleSetHeightPercent(IntPtr node, float height);

        [DllImport(YogaDllName, EntryPoint = "YGNodeStyleSetHeightAuto")]
        public static extern void YGNodeStyleSetHeightAuto(IntPtr node);

        [DllImport(YogaDllName, EntryPoint = "YGNodeStyleSetMinWidth")]
        public static extern void YGNodeStyleSetMinWidth(IntPtr node, float minWidth);

        [DllImport(YogaDllName, EntryPoint = "YGNodeStyleSetMinWidthPercent")]
        public static extern void YGNodeStyleSetMinWidthPercent(IntPtr node, float minWidth);

        [DllImport(YogaDllName, EntryPoint = "YGNodeStyleSetMinHeight")]
        public static extern void YGNodeStyleSetMinHeight(IntPtr node, float minHeight);

        [DllImport(YogaDllName, EntryPoint = "YGNodeStyleSetMinHeightPercent")]
        public static extern void YGNodeStyleSetMinHeightPercent(IntPtr node, float minHeight);

        [DllImport(YogaDllName, EntryPoint = "YGNodeStyleSetMaxWidth")]
        public static extern void YGNodeStyleSetMaxWidth(IntPtr node, float maxWidth);

        [DllImport(YogaDllName, EntryPoint = "YGNodeStyleSetMaxWidthPercent")]
        public static extern void YGNodeStyleSetMaxWidthPercent(IntPtr node, float maxWidth);

        [DllImport(YogaDllName, EntryPoint = "YGNodeStyleSetMaxHeight")]
        public static extern void YGNodeStyleSetMaxHeight(IntPtr node, float maxHeight);

        [DllImport(YogaDllName, EntryPoint = "YGNodeStyleSetMaxHeightPercent")]
        public static extern void YGNodeStyleSetMaxHeightPercent(IntPtr node, float maxHeight);

        [DllImport(YogaDllName, EntryPoint = "YGNodeStyleSetGap")]
        public static extern void YGNodeStyleSetGap(IntPtr node, Gutter gutter, float gap);

        [DllImport(YogaDllName, EntryPoint = "YGNodeStyleSetGapPercent")]
        public static extern void YGNodeStyleSetGapPercent(IntPtr node, Gutter gutter, float gap);

        [DllImport(YogaDllName, EntryPoint = "YGNodeStyleSetAspectRatio")]
        public static extern void YGNodeStyleSetAspectRatio(IntPtr node, float aspectRatio);

        [DllImport(YogaDllName, EntryPoint = "YGNodeStyleSetDisplay")]
        public static extern void YGNodeStyleSetDisplay(IntPtr node, Display display);

        [DllImport(YogaDllName, EntryPoint = "YGNodeStyleGetDisplay")]
        public static extern Display YGNodeStyleGetDisplay(IntPtr node);

        #endregion
    }
}