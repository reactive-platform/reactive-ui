using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Reactive {
    /// <summary>
    /// Represents an object that can be controlled by a layout. This abstraction assumes that alongside with <see cref="IEquatable{T}"/> you
    /// will override base methods <see cref="object.Equals(object)"/> and <see cref="object.GetHashCode()"/>, otherwise the behaviour is undefined.
    /// </summary>
    [PublicAPI]
    public interface ILayoutItem : IEquatable<ILayoutItem> {
        ILayoutDriver? LayoutDriver { get; set; }
        ILayoutModifier? LayoutModifier { get; set; }
        
        bool WithinLayout { get; set; }

        event Action<ILayoutItem>? ModifierUpdatedEvent;

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