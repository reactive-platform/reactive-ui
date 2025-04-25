using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Reactive {
    /// <summary>
    /// Represents a standalone layout controller.
    /// </summary>
    [PublicAPI]
    public interface ILayoutController : IContextMember {
        int ChildCount { get; }
        
        event Action? LayoutControllerUpdatedEvent;

        /// <summary>
        /// Adds a child to the controller.
        /// </summary>
        /// <param name="comp">A component to add.</param>
        /// <param name="index">An index to insert at.</param>
        void InsertChild(ILayoutItem comp, int index);

        /// <summary>
        /// Removes a child from the controller.
        /// </summary>
        /// <param name="comp">A component to remove.</param>
        void RemoveChild(ILayoutItem comp);

        /// <summary>
        /// Removes all children from the controller.
        /// </summary>
        void RemoveAllChildren();

        /// <summary>
        /// Checks if the controller has the specified child added to its hierarchy.
        /// </summary>
        /// <param name="comp">A component to search for.</param>
        /// <returns>True if the component is in hierarchy, otherwise False.</returns>
        bool HasChild(ILayoutItem comp);
        
        /// <summary>
        /// Applies calculated layout to first-level children. Own size is not applied.
        /// </summary>
        void ApplyChildren();
        
        /// <summary>
        /// Recalculates the layout starting from this node and applies its own size immediately after recalculation.
        /// Must be called for the root node only, otherwise the behaviour is undefined. 
        /// </summary>
        /// <param name="item">The controller as an item.</param>
        void Recalculate(ILayoutItem item, Vector2 constraints);
    }
}