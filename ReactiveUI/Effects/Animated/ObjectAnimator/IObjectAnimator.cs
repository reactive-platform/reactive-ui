using System;
using JetBrains.Annotations;

namespace Reactive {
    /// <summary>
    /// An abstraction level for animation modules. Usually used for components which
    /// are intended to control the animation behaviour.
    /// <code>
    /// new Component {
    ///     Animator = Animate((x, y) => x.ContentTransform.localScale = y * Vector3.one);
    /// }
    /// </code>
    /// </summary>
    /// <typeparam name="T">A component type to animate</typeparam>
    [PublicAPI]
    public interface IObjectAnimator<in T> {
        bool IsFinished { get; }
        float Progress { get; }

        event Action? AnimationFinishedEvent;

        /// <summary>
        /// Starts the animation.
        /// </summary>
        /// <param name="instance">The instance of an object you want to animate.</param>
        /// <param name="resetProgress">In case you stopped the animation before its completion
        /// you can either reset the progress or continue with the current one.</param>
        void StartAnimation(T instance, bool resetProgress = true);

        /// <summary>
        /// Stops the animation.
        /// </summary>
        /// <param name="finish">Finish the animation immediately or leave it as is.</param>
        void StopAnimation(bool finish = false);
    }
}