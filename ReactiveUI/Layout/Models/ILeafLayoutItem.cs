using System;
using JetBrains.Annotations;
using Reactive.Yoga;
using UnityEngine;

namespace Reactive;

/// <summary>
/// Represents a leaf layout object that is capable of controlling itself.
/// If you inherit this abstraction and try to attach a child within the same native node, you will get an error.
/// </summary>
[PublicAPI]
public interface ILeafLayoutItem : ILayoutItem {
    /// <summary>
    /// Invoked when the leaf item is updated and requires layout recalculation.
    /// </summary>
    event Action<ILeafLayoutItem>? LeafLayoutUpdatedEvent;
    
    /// <summary>
    /// Returns the computed dimensions of the node, following the constraints of widthMode and heightMode.
    /// </summary>
    /// <returns>The dimensions vector.</returns>
    Vector2 Measure(float width, MeasureMode widthMode, float height, MeasureMode heightMode);
}