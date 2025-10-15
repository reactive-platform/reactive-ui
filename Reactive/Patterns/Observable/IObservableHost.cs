using System;
using JetBrains.Annotations;

namespace Reactive {
    /// <summary>
    /// Represents an observable object.
    /// </summary>
    [PublicAPI]
    public interface IObservableHost {
        /// <summary>
        /// Invokes when a property is changed. Passes a property name and a value in arguments.
        /// Use this when you need to listen for more than one property using the same callback.
        /// </summary>
        event Action<string, object>? PropertyChangedEvent; 
        
        /// <summary>
        /// Adds a listener for the specific property.
        /// </summary>
        /// <param name="propertyName">A property to listen for.</param>
        /// <param name="callback">A callback to invoke when the property is updated.</param>
        /// <typeparam name="T">A property type.</typeparam>
        void AddCallback<T>(string propertyName, Action<T> callback);
        
        /// <summary>
        /// Removes a listener for the specific property.
        /// </summary>
        /// <param name="propertyName">A property to remove the listener from.</param>
        /// <param name="callback">A callback to remove.</param>
        /// <typeparam name="T">A property type.</typeparam>
        void RemoveCallback<T>(string propertyName, Action<T> callback);
    }
}