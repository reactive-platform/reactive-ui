using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Reactive {
    internal class ObservableHost(object keeper) : IObservableHost {
        private readonly Dictionary<string, Delegate> _delegates = new();
        private readonly Dictionary<string, PropertyInfo> _properties = new();

        public event Action<string, object>? PropertyChangedEvent;

        public void AddCallback<T>(string propertyName, Action<T> callback) {
            if (_delegates.TryGetValue(propertyName, out var del)) {
                del = Delegate.Combine(del, callback);
            } else {
                del = callback;
            }

            _delegates[propertyName] = del;
        }

        public void RemoveCallback<T>(string propertyName, Action<T> callback) {
            if (!_delegates.TryGetValue(propertyName, out var del)) {
                return;
            }

            del = Delegate.Remove(del, callback);
            _delegates[propertyName] = del;
        }

        public void NotifyPropertyChanged(string name, Optional<object> value) {
            if (!_delegates.TryGetValue(name, out var del) && PropertyChangedEvent == null) {
                return;
            }

            if (!value.HasValue) {
                // Fetch and cache a property instance if not presented
                if (!_properties.TryGetValue(name, out var prop)) {
                    prop = keeper.GetType().GetProperty(name, ReflectionUtils.DefaultFlags);

                    if (prop == null) {
                        return;
                    }

                    _properties[name] = prop;
                }

                // Get a value if not presented
                if (prop.GetMethod is not { } getter) {
                    throw new Exception("Notifiable property must have a getter method to work");
                }

                value = getter.Invoke(keeper, []);
            }

            var arg = value.Value;

            // Invoke the callback
            try {
                del?.DynamicInvoke(arg);
                PropertyChangedEvent?.Invoke(name, arg!);
            } catch (Exception ex) {
                Debug.unityLogger.Log(LogType.Exception, $"Failed to invoke callback: \n{ex}");
            }
        }
    }
}