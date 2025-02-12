using System;
using System.Runtime.InteropServices;

namespace Reactive.Yoga {
    internal static class YogaNative {
        static YogaNative() {
            NativeLibraryLoader.Initialize();
            YogaConfig.Default.SetPointScaleFactor(0f);
        }

        private const string DllName = "__Internal";

        #region YGBindings

        public enum YGLogLevel {
            YGLogLevelError,
            YGLogLevelWarn,
            YGLogLevelInfo,
            YGLogLevelDebug,
            YGLogLevelVerbose,
            YGLogLevelFatal
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void YGLoggerDelegate(string message, YGLogLevel logLevel);

        [DllImport(DllName, EntryPoint = "YGBindingsSetLogger", CallingConvention = CallingConvention.Cdecl)]
        private static extern void YGBindingsSetLoggerInternal(YGLoggerDelegate callback);

        public static void YGBindingsSetLogger(YGLoggerDelegate callback) {
            _ = NativeLibraryLoader.LibraryPath;
            YGBindingsSetLoggerInternal(callback);
        }

        #endregion

        #region YGConfig

        [DllImport(DllName, EntryPoint = "YGConfigGetDefault", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr YGConfigGetDefaultInternal();

        public static IntPtr YGConfigGetDefault() {
            _ = NativeLibraryLoader.LibraryPath;
            return YGConfigGetDefaultInternal();
        }

        [DllImport(DllName, EntryPoint = "YGConfigSetPointScaleFactor", CallingConvention = CallingConvention.Cdecl)]
        private static extern void YGConfigSetPointScaleFactorInternal(IntPtr ptr, float factor);

        public static void YGConfigSetPointScaleFactor(IntPtr ptr, float factor) {
            _ = NativeLibraryLoader.LibraryPath;
            YGConfigSetPointScaleFactorInternal(ptr, factor);
        }

        #endregion

        #region YGNode

        [DllImport(DllName, EntryPoint = "YGNodeNew", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr YGNodeNewInternal();

        public static IntPtr YGNodeNew() {
            _ = NativeLibraryLoader.LibraryPath;
            return YGNodeNewInternal();
        }

        [DllImport(DllName, EntryPoint = "YGNodeFree", CallingConvention = CallingConvention.Cdecl)]
        private static extern void YGNodeFreeInternal(IntPtr node);

        public static void YGNodeFree(IntPtr node) {
            _ = NativeLibraryLoader.LibraryPath;
            YGNodeFreeInternal(node);
        }

        [DllImport(DllName, EntryPoint = "YGNodeCalculateLayout", CallingConvention = CallingConvention.Cdecl)]
        private static extern void YGNodeCalculateLayoutInternal(IntPtr node, float availableWidth, float availableHeight, Direction ownerDirection);

        public static void YGNodeCalculateLayout(IntPtr node, float availableWidth, float availableHeight, Direction ownerDirection) {
            _ = NativeLibraryLoader.LibraryPath;
            YGNodeCalculateLayoutInternal(node, availableWidth, availableHeight, ownerDirection);
        }

        [DllImport(DllName, EntryPoint = "YGNodeInsertChild", CallingConvention = CallingConvention.Cdecl)]
        private static extern void YGNodeInsertChildInternal(IntPtr node, IntPtr child, int index);

        public static void YGNodeInsertChildSafe(IntPtr node, IntPtr child, int index) {
            _ = NativeLibraryLoader.LibraryPath;
            YGNodeInsertChildInternal(node, child, index);
        }

        [DllImport(DllName, EntryPoint = "YGNodeRemoveChild", CallingConvention = CallingConvention.Cdecl)]
        private static extern void YGNodeRemoveChildInternal(IntPtr node, IntPtr child);

        public static void YGNodeRemoveChildSafe(IntPtr node, IntPtr child) {
            _ = NativeLibraryLoader.LibraryPath;
            YGNodeRemoveChildInternal(node, child);
        }

        [DllImport(DllName, EntryPoint = "YGNodeRemoveAllChildren", CallingConvention = CallingConvention.Cdecl)]
        private static extern void YGNodeRemoveAllChildrenInternal(IntPtr node);

        public static void YGNodeRemoveAllChildrenSafe(IntPtr node) {
            _ = NativeLibraryLoader.LibraryPath;
            YGNodeRemoveAllChildrenInternal(node);
        }

        #endregion

        #region YGNodeLayout

        [DllImport(DllName, EntryPoint = "YGNodeLayoutGetLeft", CallingConvention = CallingConvention.Cdecl)]
        private static extern float YGNodeLayoutGetLeftInternal(IntPtr node);

        public static float YGNodeLayoutGetLeft(IntPtr node) {
            _ = NativeLibraryLoader.LibraryPath;
            return YGNodeLayoutGetLeftInternal(node);
        }

        [DllImport(DllName, EntryPoint = "YGNodeLayoutGetTop", CallingConvention = CallingConvention.Cdecl)]
        private static extern float YGNodeLayoutGetTopInternal(IntPtr node);

        public static float YGNodeLayoutGetTop(IntPtr node) {
            _ = NativeLibraryLoader.LibraryPath;
            return YGNodeLayoutGetTopInternal(node);
        }

        [DllImport(DllName, EntryPoint = "YGNodeLayoutGetWidth", CallingConvention = CallingConvention.Cdecl)]
        private static extern float YGNodeLayoutGetWidthInternal(IntPtr node);

        public static float YGNodeLayoutGetWidth(IntPtr node) {
            _ = NativeLibraryLoader.LibraryPath;
            return YGNodeLayoutGetWidthInternal(node);
        }

        [DllImport(DllName, EntryPoint = "YGNodeLayoutGetHeight", CallingConvention = CallingConvention.Cdecl)]
        private static extern float YGNodeLayoutGetHeightInternal(IntPtr node);

        public static float YGNodeLayoutGetHeight(IntPtr node) {
            _ = NativeLibraryLoader.LibraryPath;
            return YGNodeLayoutGetHeightInternal(node);
        }

        #endregion

        #region YGNodeStyle

        [DllImport(DllName, EntryPoint = "YGNodeStyleSetOverflow", CallingConvention = CallingConvention.Cdecl)]
        private static extern void YGNodeStyleSetOverflowInternal(IntPtr node, Overflow overflow);
        public static void YGNodeStyleSetOverflow(IntPtr node, Overflow overflow) {
            _ = NativeLibraryLoader.LibraryPath;
            YGNodeStyleSetOverflowInternal(node, overflow);
        }

        [DllImport(DllName, EntryPoint = "YGNodeStyleSetDirection", CallingConvention = CallingConvention.Cdecl)]
        private static extern void YGNodeStyleSetDirectionInternal(IntPtr node, Direction direction);
        public static void YGNodeStyleSetDirection(IntPtr node, Direction direction) {
            _ = NativeLibraryLoader.LibraryPath;
            YGNodeStyleSetDirectionInternal(node, direction);
        }

        [DllImport(DllName, EntryPoint = "YGNodeStyleSetFlexDirection")]
        public static extern void YGNodeStyleSetFlexDirection(IntPtr node, FlexDirection flexDirection);

        [DllImport(DllName, EntryPoint = "YGNodeStyleSetJustifyContent")]
        public static extern void YGNodeStyleSetJustifyContent(IntPtr node, Justify justifyContent);

        [DllImport(DllName, EntryPoint = "YGNodeStyleSetAlignContent")]
        public static extern void YGNodeStyleSetAlignContent(IntPtr node, Align alignContent);

        [DllImport(DllName, EntryPoint = "YGNodeStyleSetAlignItems")]
        public static extern void YGNodeStyleSetAlignItems(IntPtr node, Align alignItems);

        [DllImport(DllName, EntryPoint = "YGNodeStyleSetAlignSelf")]
        public static extern void YGNodeStyleSetAlignSelf(IntPtr node, Align alignSelf);

        [DllImport(DllName, EntryPoint = "YGNodeStyleSetPositionType")]
        public static extern void YGNodeStyleSetPositionType(IntPtr node, PositionType positionType);

        [DllImport(DllName, EntryPoint = "YGNodeStyleSetFlexWrap")]
        public static extern void YGNodeStyleSetFlexWrap(IntPtr node, Wrap flexWrap);

        [DllImport(DllName, EntryPoint = "YGNodeStyleSetFlexGrow")]
        public static extern void YGNodeStyleSetFlexGrow(IntPtr node, float flexGrow);

        [DllImport(DllName, EntryPoint = "YGNodeStyleSetFlexShrink")]
        public static extern void YGNodeStyleSetFlexShrink(IntPtr node, float flexShrink);

        [DllImport(DllName, EntryPoint = "YGNodeStyleSetFlexBasis")]
        public static extern void YGNodeStyleSetFlexBasis(IntPtr node, float flexBasis);

        [DllImport(DllName, EntryPoint = "YGNodeStyleSetFlexBasisPercent")]
        public static extern void YGNodeStyleSetFlexBasisPercent(IntPtr node, float flexBasis);

        [DllImport(DllName, EntryPoint = "YGNodeStyleSetFlexBasisAuto")]
        public static extern void YGNodeStyleSetFlexBasisAuto(IntPtr node);

        [DllImport(DllName, EntryPoint = "YGNodeStyleSetPosition")]
        public static extern void YGNodeStyleSetPosition(IntPtr node, Edge edge, float position);

        [DllImport(DllName, EntryPoint = "YGNodeStyleSetPositionPercent")]
        public static extern void YGNodeStyleSetPositionPercent(IntPtr node, Edge edge, float position);

        [DllImport(DllName, EntryPoint = "YGNodeStyleSetMargin")]
        public static extern void YGNodeStyleSetMargin(IntPtr node, Edge edge, float margin);

        [DllImport(DllName, EntryPoint = "YGNodeStyleSetMarginPercent")]
        public static extern void YGNodeStyleSetMarginPercent(IntPtr node, Edge edge, float margin);

        [DllImport(DllName, EntryPoint = "YGNodeStyleSetMarginAuto")]
        public static extern void YGNodeStyleSetMarginAuto(IntPtr node, Edge edge);

        [DllImport(DllName, EntryPoint = "YGNodeStyleSetPadding")]
        public static extern void YGNodeStyleSetPadding(IntPtr node, Edge edge, float padding);

        [DllImport(DllName, EntryPoint = "YGNodeStyleSetPaddingPercent")]
        public static extern void YGNodeStyleSetPaddingPercent(IntPtr node, Edge edge, float padding);

        [DllImport(DllName, EntryPoint = "YGNodeStyleGetWidth")]
        public static extern YogaValue YGNodeStyleGetWidth(IntPtr node);

        [DllImport(DllName, EntryPoint = "YGNodeStyleSetWidth")]
        public static extern void YGNodeStyleSetWidth(IntPtr node, float width);

        [DllImport(DllName, EntryPoint = "YGNodeStyleSetWidthPercent")]
        public static extern void YGNodeStyleSetWidthPercent(IntPtr node, float width);

        [DllImport(DllName, EntryPoint = "YGNodeStyleSetWidthAuto")]
        public static extern void YGNodeStyleSetWidthAuto(IntPtr node);

        [DllImport(DllName, EntryPoint = "YGNodeStyleGetHeight")]
        public static extern YogaValue YGNodeStyleGetHeight(IntPtr node);

        [DllImport(DllName, EntryPoint = "YGNodeStyleSetHeight")]
        public static extern void YGNodeStyleSetHeight(IntPtr node, float height);

        [DllImport(DllName, EntryPoint = "YGNodeStyleSetHeightPercent")]
        public static extern void YGNodeStyleSetHeightPercent(IntPtr node, float height);

        [DllImport(DllName, EntryPoint = "YGNodeStyleSetHeightAuto")]
        public static extern void YGNodeStyleSetHeightAuto(IntPtr node);

        [DllImport(DllName, EntryPoint = "YGNodeStyleSetMinWidth")]
        public static extern void YGNodeStyleSetMinWidth(IntPtr node, float minWidth);

        [DllImport(DllName, EntryPoint = "YGNodeStyleSetMinWidthPercent")]
        public static extern void YGNodeStyleSetMinWidthPercent(IntPtr node, float minWidth);

        [DllImport(DllName, EntryPoint = "YGNodeStyleSetMinHeight")]
        public static extern void YGNodeStyleSetMinHeight(IntPtr node, float minHeight);

        [DllImport(DllName, EntryPoint = "YGNodeStyleSetMinHeightPercent")]
        public static extern void YGNodeStyleSetMinHeightPercent(IntPtr node, float minHeight);

        [DllImport(DllName, EntryPoint = "YGNodeStyleSetMaxWidth")]
        public static extern void YGNodeStyleSetMaxWidth(IntPtr node, float maxWidth);

        [DllImport(DllName, EntryPoint = "YGNodeStyleSetMaxWidthPercent")]
        public static extern void YGNodeStyleSetMaxWidthPercent(IntPtr node, float maxWidth);

        [DllImport(DllName, EntryPoint = "YGNodeStyleSetMaxHeight")]
        public static extern void YGNodeStyleSetMaxHeight(IntPtr node, float maxHeight);

        [DllImport(DllName, EntryPoint = "YGNodeStyleSetMaxHeightPercent")]
        public static extern void YGNodeStyleSetMaxHeightPercent(IntPtr node, float maxHeight);

        [DllImport(DllName, EntryPoint = "YGNodeStyleSetGap")]
        public static extern void YGNodeStyleSetGap(IntPtr node, Gutter gutter, float gap);

        [DllImport(DllName, EntryPoint = "YGNodeStyleSetGapPercent")]
        public static extern void YGNodeStyleSetGapPercent(IntPtr node, Gutter gutter, float gap);

        [DllImport(DllName, EntryPoint = "YGNodeStyleSetAspectRatio")]
        public static extern void YGNodeStyleSetAspectRatio(IntPtr node, float aspectRatio);

        [DllImport(DllName, EntryPoint = "YGNodeStyleSetDisplay")]
        public static extern void YGNodeStyleSetDisplay(IntPtr node, Display display);

        [DllImport(DllName, EntryPoint = "YGNodeStyleGetDisplay")]
        public static extern Display YGNodeStyleGetDisplay(IntPtr node);

        #endregion
    }
}