using System;
using JetBrains.Annotations;

namespace Reactive {
    /// <summary>
    /// Represents a modifier object that is used to define a component properties in a layout.
    /// </summary>
    [PublicAPI]
    public interface ILayoutModifier : ICopiable<ILayoutModifier>, IContextMember {
        event Action? ModifierUpdatedEvent;

        /// <summary>
        /// Exposes a layout item in case the modifier needs to access impl-specific properties.
        /// </summary>
        /// <param name="item">The item modifier belongs to.</param>
        void ExposeLayoutItem(ILayoutItem? item);
    }
}