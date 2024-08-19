using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Reactive {
    internal class ObservableHost(object keeper) : IObservableHost {
        private readonly Dictionary<string, Delegate> _delegates = new();
        private readonly Dictionary<string, PropertyInfo> _properties = new();

        public void AddCallback<T>(string propertyName, Action<T> callback) {
            if (_delegates.TryGetValue(propertyName, out var del)) {
                del = Delegate.Combine(del, callback);
            } else {
                del = callback;
            }
            _delegates[propertyName] = del;
        }

        public void RemoveCallback<T>(string propertyName, Action<T> callback) {
            if (!_delegates.TryGetValue(propertyName, out var del)) return;
            del = Delegate.Remove(del, callback);
            _delegates[propertyName] = del;
        }

        public void NotifyPropertyChanged(string? name = null) {
            if (name == null || !_delegates.TryGetValue(name, out var del)) return;

            if (!_properties.TryGetValue(name, out var prop)) {
                prop = keeper.GetType().GetProperty(name, ReflectionUtils.DefaultFlags);
                if (prop == null) return;
                _properties[name] = prop;
            }
            var getter = prop.GetMethod;
            if (getter == null) {
                throw new Exception("Notifiable property must have a getter method to work");
            }

            try {
                var value = getter.Invoke(keeper, Array.Empty<object>());
                del.DynamicInvoke(value);
            } catch (Exception ex) {
                Debug.LogError($"Failed to invoke callback: \n{ex}");
            }
        }
    }
}