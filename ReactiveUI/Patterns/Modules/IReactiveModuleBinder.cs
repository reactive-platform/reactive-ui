using System.Collections.Generic;
using JetBrains.Annotations;

namespace Reactive {
    /// <summary>
    /// Represents a component alignable for module binding. Available for any component that derives <see cref="ReactiveComponent"/>.
    /// </summary>
    [PublicAPI]
    public interface IReactiveModuleBinder {
        /// <summary>
        /// Provides access to all bound modules.
        /// </summary>
        IReadOnlyCollection<IReactiveModule> Modules { get; }

        /// <summary>
        /// Binds a specified module.
        /// </summary>
        /// <param name="module">A module to bind.</param>
        void BindModule(IReactiveModule module);
        
        /// <summary>
        /// Unbinds a specified module.
        /// </summary>
        /// <param name="module">A module to unbind.</param>
        void UnbindModule(IReactiveModule module);
    }
}