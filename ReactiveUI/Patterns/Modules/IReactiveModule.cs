using JetBrains.Annotations;

namespace Reactive {
    /// <summary>
    /// Represents a module object that can be registered into the component via <see cref="IReactiveModuleBinder"/> interface.
    /// </summary>
    [PublicAPI]
    public interface IReactiveModule {
        /// <summary>
        /// Called on each Update cycle if a component is enabled.
        /// </summary>
        void OnUpdate();
        
        /// <summary>
        /// Called after binding to a component.
        /// </summary>
        void OnBind();
        
        /// <summary>
        /// Called after unbinding from a component.
        /// </summary>
        void OnUnbind();
    }
}