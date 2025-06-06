﻿using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Reactive {
    [PublicAPI]
    public static class ReactiveComponentExtensions {
        public static T With<T>(this T comp, Action<T> action) where T : IReactiveComponent {
            action(comp);
            return comp;
        }
        
        public static T WithNativeComponent<T, TComp>(this T comp, out TComp ncomp) where T : IReactiveComponent where TComp : Component {
            ncomp = comp.Content.AddComponent<TComp>();
            return comp;
        }

        public static T WithModifier<T>(this T comp, ILayoutModifier? modifier) where T : ILayoutItem {
            comp.LayoutModifier = modifier;
            return comp;
        }

        public static T WithoutModifier<T>(this T comp) where T : ILayoutItem {
            return comp.WithModifier(null);
        }

        public static T Bind<T, TBind>(this T comp, ref TBind variable) where T : TBind, IReactiveComponent {
            variable = comp;
            return comp;
        }
        
        public static T Export<T>(this T comp, out T variable) where T : IReactiveComponent {
            variable = comp;
            return comp;
        }
        
        public static T Bind<T>(this T comp, ref RectTransform variable) where T : IReactiveComponent {
            variable = comp.ContentTransform;
            return comp;
        }

        public static RectTransform Use(this ReactiveComponent comp, GameObject parent) {
            comp.Use(parent.GetOrAddComponent<RectTransform>());
            return comp.ContentTransform;
        }
    }
}