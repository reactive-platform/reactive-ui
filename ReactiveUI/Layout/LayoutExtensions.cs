using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Reactive.Yoga;
using UnityEngine;

namespace Reactive {
    [PublicAPI]
    public static class LayoutExtensions {
        #region Flex Item

        public static T AsFlexItem<T>(
            this T component,
            out YogaModifier modifier,
            float? flex = null,
            float? flexGrow = null,
            float? flexShrink = null,
            YogaValue? basis = null,
            YogaVector? size = null,
            YogaVector? minSize = null,
            YogaVector? maxSize = null,
            YogaFrame? margin = null,
            float? aspectRatio = null,
            YogaFrame? position = null,
            PositionType? positionType = null,
            Align alignSelf = Align.Auto
        ) where T : ILayoutItem {
            return AsFlexItem(
                component,
                component.LayoutModifier,
                static (comp, mod) => comp.LayoutModifier = mod,
                out modifier,
                flex,
                flexGrow,
                flexShrink,
                basis,
                size,
                minSize,
                maxSize,
                margin,
                aspectRatio,
                position,
                positionType,
                alignSelf
            );
        }

        public static T AsFlexItem<T>(
            this T component,
            float? flex = null,
            float? flexGrow = null,
            float? flexShrink = null,
            YogaValue? basis = null,
            YogaVector? size = null,
            YogaVector? minSize = null,
            YogaVector? maxSize = null,
            YogaFrame? margin = null,
            float? aspectRatio = null,
            YogaFrame? position = null,
            PositionType? positionType = null,
            Align alignSelf = Align.Auto
        ) where T : ILayoutItem {
            return AsFlexItem(
                component,
                out _,
                flex,
                flexGrow,
                flexShrink,
                basis,
                size,
                minSize,
                maxSize,
                margin,
                aspectRatio,
                position,
                positionType,
                alignSelf
            );
        }

        public static T AsFlexItem<T>(
            this T component,
            ILayoutModifier? layoutModifier,
            Action<T, ILayoutModifier> setModifierCallback,
            out YogaModifier yogaModifier,
            float? flex = null,
            float? flexGrow = null,
            float? flexShrink = null,
            YogaValue? basis = null,
            YogaVector? size = null,
            YogaVector? minSize = null,
            YogaVector? maxSize = null,
            YogaFrame? margin = null,
            float? aspectRatio = null,
            YogaFrame? position = null,
            PositionType? positionType = null,
            Align alignSelf = Align.Auto
        ) where T : ILayoutItem {
            if (layoutModifier is not YogaModifier modifier) {
                modifier = new YogaModifier();
                setModifierCallback(component, modifier);
            }
            
            if (position != null) {
                modifier.PositionType = PositionType.Absolute;
                modifier.Position = position.Value;
            }
            
            if (positionType != null) {
                modifier.PositionType = positionType.Value;
            }
            
            if (size != null) {
                modifier.Size = size.Value;
            }
            
            if (flex != null) {
                modifier.Flex = flex.Value;
            }
            
            if (flexShrink != null) {
                modifier.FlexShrink = flexShrink.Value;
            }
            
            if (flexGrow != null) {
                modifier.FlexGrow = flexGrow.Value;
            }
            
            if (basis != null) {
                modifier.FlexBasis = basis.Value;
            }
            
            if (minSize != null) {
                modifier.MinSize = minSize.Value;
            }
            
            if (maxSize != null) {
                modifier.MaxSize = maxSize.Value;
            }
            
            if (margin != null) {
                modifier.Margin = margin.Value;
            }
            
            if (aspectRatio != null) {
                modifier.AspectRatio = aspectRatio.Value;
            }
            
            modifier.AlignSelf = alignSelf;
            yogaModifier = modifier;
            return component;
        }

        #endregion

        #region Flex Group

        public static T AsFlexGroup<T>(
            this T component,
            FlexDirection? direction = null,
            Justify? justifyContent = null,
            Align? alignItems = null,
            Align? alignContent = null,
            Wrap? wrap = null,
            Overflow? overflow = null,
            YogaFrame? padding = null,
            YogaVector? gap = null,
            bool constrainHorizontal = true,
            bool constrainVertical = true
        ) where T : ILayoutDriver {
            return AsFlexGroup(
                component,
                out _,
                direction,
                justifyContent,
                alignItems,
                alignContent,
                wrap,
                overflow,
                padding,
                gap,
                constrainHorizontal,
                constrainVertical
            );
        }

        public static T AsFlexGroup<T>(
            this T component,
            out YogaLayoutController layoutController,
            FlexDirection? direction = null,
            Justify? justifyContent = null,
            Align? alignItems = null,
            Align? alignContent = null,
            Wrap? wrap = null,
            Overflow? overflow = null,
            YogaFrame? padding = null,
            YogaVector? gap = null,
            bool constrainHorizontal = true,
            bool constrainVertical = true
        ) where T : ILayoutDriver {
            if (component.LayoutController is not YogaLayoutController controller) {
                controller = new();
                component.LayoutController = controller;
            }
            
            if (direction.HasValue) {
                controller.FlexDirection = direction.Value;
            }
            
            if (justifyContent.HasValue) {
                controller.JustifyContent = justifyContent.Value;
            }
            
            if (alignContent.HasValue) {
                controller.AlignContent = alignContent.Value;
            }
            
            if (alignItems.HasValue) {
                controller.AlignItems = alignItems.Value;
            }
            
            if (wrap.HasValue) {
                controller.FlexWrap = wrap.Value;
            }
            
            if (overflow.HasValue) {
                controller.Overflow = overflow.Value;
            }
            
            if (padding.HasValue) {
                controller.Padding = padding.Value;
            }
            
            if (gap.HasValue) {
                controller.Gap = gap.Value;
            }

            // Those aren't native properties so we don't care
            controller.ConstrainVertical = constrainVertical;
            controller.ConstrainHorizontal = constrainHorizontal;

            layoutController = controller;
            return component;
        }

        #endregion

        #region Rect

        public static T WithSizeDelta<T>(
            this T component,
            float width,
            float height
        ) where T : IReactiveComponent {
            return component.AsRectItem(new(width, height));
        }

        public static T AsRectItem<T>(
            this T component,
            Vector2? sizeDelta = null,
            Vector2? anchorMin = null,
            Vector2? anchorMax = null,
            Vector2? pivot = null
        ) where T : IReactiveComponent {
            var transform = component.ContentTransform;
            transform.anchorMin = anchorMin ?? transform.anchorMin;
            transform.anchorMax = anchorMax ?? transform.anchorMax;
            transform.sizeDelta = sizeDelta ?? transform.sizeDelta;
            transform.pivot = pivot ?? transform.pivot;
            return component;
        }

        public static T WithRectExpand<T>(this T component) where T : IReactiveComponent {
            component.ContentTransform.WithRectExpand();
            return component;
        }

        public static ILayoutItem WithRectExpand(this ILayoutItem component) {
            component.BeginApply().WithRectExpand();
            component.EndApply();
            return component;
        }

        public static RectTransform WithRectExpand(this RectTransform component) {
            component.anchorMin = Vector2.zero;
            component.anchorMax = Vector2.one;
            component.sizeDelta = Vector2.zero;
            component.anchoredPosition = Vector2.zero;
            return component;
        }

        #endregion

        public static T With<T>(this T modifier, ILayoutModifier apModifier) where T : ILayoutModifier {
            modifier.CopyFrom(apModifier);
            return modifier;
        }

        /// <summary>
        /// Finds index of an item in the <see cref="ILayoutItem"/> list. You must use this instead of the
        /// default method, otherwise the behaviour is undefined.
        /// </summary>
        /// <param name="items">A list to search in.</param>
        /// <param name="item">An item to search for.</param>
        /// <returns>An index of the item, or -1 if not presented.</returns>
        public static int FindLayoutItemIndex(this IList<ILayoutItem> items, ILayoutItem item) {
            for (var i = 0; i < items.Count; i++) {
                if (items[i].EqualsToLayoutItem(item)) {
                    return i;
                }
            }

            return -1;
        }
    }
}