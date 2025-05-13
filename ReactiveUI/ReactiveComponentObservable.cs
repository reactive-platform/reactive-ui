using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Reactive;

public partial class ReactiveComponent {
    #region Observable

    private class PropertyRouter(ObservableHost router, IObservableHost entry, string? prefix) {
        public void Init() {
            entry.PropertyChangedEvent += HandlePropertyChanged;
        }

        public void Deinit() {
            entry.PropertyChangedEvent -= HandlePropertyChanged;
        }

        private void HandlePropertyChanged(string propertyName, object value) {
            if (prefix != null) {
                propertyName = $"{prefix}.{propertyName}";
            }
            
            router.NotifyPropertyChanged(propertyName, value);
        }
    }

    private Dictionary<IObservableHost, PropertyRouter>? _observableRouters;

    /// <summary>
    /// Subscribes to a nested component updates and routes them through itself.
    /// </summary>
    /// <param name="component">A component to route from.</param>
    /// <param name="prefix">A prefix to add to routed property names. Pass null to leave them as is.</param>
    protected void RoutePropertyChanged(IObservableHost component, string? prefix) {
        _observableRouters ??= new();

        var router = new PropertyRouter(_observableHost, component, prefix);
        router.Init();

        _observableRouters.Add(component, router);
    }
    
    /// <summary>
    /// Unsubscribes from a nested component updates and stops routing its properties.
    /// </summary>
    /// <param name="component">A component to remove routing from.</param>
    protected void RemoveRoutePropertyChanged(IObservableHost component) {
        if (_observableRouters?.TryGetValue(component, out var router) ?? false) {
            router.Deinit();
            _observableRouters.Remove(component);
        }
    }

    #endregion

    #region Impl

    public event Action<string, object>? PropertyChangedEvent {
        add => _observableHost.PropertyChangedEvent += value;
        remove => _observableHost.PropertyChangedEvent -= value;
    }

    private ObservableHost _observableHost = null!;

    protected void NotifyPropertyChanged([CallerMemberName] string? name = null) {
        if (name == null) {
            throw new InvalidOperationException("Name must have a value");
        }
        
        _observableHost.NotifyPropertyChanged(name, default);
    }

    public void AddCallback<T>(string propertyName, Action<T> callback) {
        _observableHost.AddCallback(propertyName, callback);
    }

    public void RemoveCallback<T>(string propertyName, Action<T> callback) {
        _observableHost.RemoveCallback(propertyName, callback);
    }

    #endregion
}