using System.Collections.Generic;
using JetBrains.Annotations;

namespace Reactive {
    [PublicAPI]
    public interface IAnimationHost {
        IReadOnlyCollection<IAnimation> Animations { get; }
        
        /// <summary>
        /// Adds an animation and starts to execute it immediately. 
        /// </summary>
        void AddAnimation(IAnimation animation);

        /// <summary>
        /// Pauses an animation. 
        /// </summary>
        /// <exception cref="System.ArgumentException">Throws when host does not control the specified animation.</exception>
        void PauseAnimation(IAnimation animation);
        
        /// <summary>
        /// Resumes an animation. 
        /// </summary>
        /// <exception cref="System.ArgumentException">Throws when host does not control the specified animation.</exception>
        void ResumeAnimation(IAnimation animation);
    }
}