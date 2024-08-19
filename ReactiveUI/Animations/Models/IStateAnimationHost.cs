using JetBrains.Annotations;

namespace Reactive {
    [PublicAPI]
    public interface IStateAnimationHost : IAnimationHost {
        /// <summary>
        /// Adds an animation to the specified state. The animation will start execution
        /// when host will be in the specified state.
        /// </summary>
        /// <param name="selector">State bitmask.</param>
        /// <param name="animation">An animation to add.</param>
        void AddStateAnimation(ComponentState selector, IAnimation animation);

        /// <summary>
        /// Adds a transition to the specified state. Transition is an animation that will start execution
        /// when host will be in the specified state and prevent all other animations from starting until it finishes.
        /// </summary>
        /// <param name="selector">State bitmask.</param>
        /// <param name="animation">A transition to add.</param>
        void AddStateTransition(ComponentState selector, IAnimation animation);
        
        /// <summary>
        /// Gets the state to which the animation is bound to.
        /// </summary>
        /// <returns>State to which animation is bound to or null if the animation is persistent.</returns>
        ComponentState? GetAnimationState(IAnimation animation);
    }
}