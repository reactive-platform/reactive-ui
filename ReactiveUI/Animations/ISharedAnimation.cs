using JetBrains.Annotations;

namespace Reactive;

/// <summary>
/// Represents an animation controlled by one or more objects.
/// </summary>
[PublicAPI]
public interface ISharedAnimation : IAnimation, IReactiveModule {
    /// <summary>
    /// Starts the animation.
    /// </summary>
    void Play();
}