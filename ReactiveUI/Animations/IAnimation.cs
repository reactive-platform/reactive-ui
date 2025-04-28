using System;
using JetBrains.Annotations;

namespace Reactive;

/// <summary>
/// Represents an animation controlled by a single object (usually local animations).
/// </summary>
[PublicAPI]
public interface IAnimation {
    bool IsFinished { get; }
    
    event Action? AnimationFinishedEvent;

    /// <summary>
    /// Finishes the animation and immediately evaluates to the target value.
    /// </summary>
    void FinishToEnd();

    /// <summary>
    /// Finishes the animation without reaching the target value.
    /// </summary>
    void Finish();
}