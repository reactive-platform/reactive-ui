using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Reactive {
    /// <summary>
    /// Represents an object that can be controlled by a layout.
    /// </summary>
    [PublicAPI]
    public interface ILayoutItem {
        ILayoutDriver? LayoutDriver { get; set; }
        ILayoutModifier? LayoutModifier { get; set; }
        
        bool WithinLayout { get; set; }

        event Action<ILayoutItem>? ModifierUpdatedEvent;

        /// <summary>
        /// Calculates and returns the item hash code.
        /// </summary>
        /// <returns>A hash code.</returns>
        int GetLayoutItemHashCode();

        /// <summary>
        /// Compares the current instance with another one.
        /// </summary>
        /// <param name="item">An instance to compare with.</param>
        /// <returns>True if items belong to the same host, otherwise False.</returns>
        bool EqualsToLayoutItem(ILayoutItem item);
        
        /// <summary>
        /// Starts layout application. This way is used instead of a delegate in order to reduce unnecessary allocations.
        /// </summary>
        /// <returns>A rect transform to operate on.</returns>
        RectTransform BeginApply();
        
        /// <summary>
        /// Ends layout application. This way is used instead of a delegate in order to reduce unnecessary allocations.
        /// </summary>
        void EndApply();
    }
}