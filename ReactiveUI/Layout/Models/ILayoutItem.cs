using System;
using UnityEngine;

namespace Reactive {
    public interface ILayoutItem : IEquatable<ILayoutItem> {
        ILayoutDriver? LayoutDriver { get; set; }
        ILayoutModifier? LayoutModifier { get; set; }
        
        float? DesiredHeight { get; }
        float? DesiredWidth { get; }
        bool WithinLayout { get; set; }

        event Action<ILayoutItem>? ModifierUpdatedEvent;

        void ApplyTransforms(Action<RectTransform> applicator);
    }
}