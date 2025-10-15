using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Reactive;

public partial class ReactiveComponent {
    #region Observable

    private class PropertyRouter(ObservableHost router, IObservableHost entry, string? prefix) {
        public readonly string? Prefix = prefix;

        public void Init() {
            entry.PropertyChangedEvent += HandlePropertyChanged;
        }

        public void Deinit() {
            entry.PropertyChangedEvent -= HandlePropertyChanged;
        }

        private void HandlePropertyChanged(string propertyName, object value) {
            if (Prefix != null) {
                propertyName = $"{Prefix}.{propertyName}";
            }

            router.NotifyPropertyChanged(propertyName, value);
        }
    }

    private Dictionary<IObservableHost, PropertyRouter>? _propertyRouters;
    private Dictionary<string, IObservableHost>? _propertyRoutes;

    /// <summary>
    /// Subscribes to a nested component updates and routes them through itself.
    /// </summary>
    /// <param name="component">A component to route from.</param>
    /// <param name="prefix">A prefix to add to routed property names. Pass null to leave them as is.</param>
    protected void RoutePropertyChanged(IObservableHost component, string? prefix) {
        _propertyRouters ??= new();

        var router = new PropertyRouter(_observableHost, component, prefix);
        router.Init();

        _propertyRouters.Add(component, router);

        if (prefix != null) {
            _propertyRoutes ??= new();
            _propertyRoutes.Add(prefix, component);
        }
    }

    /// <summary>
    /// Unsubscribes from a nested component updates and stops routing its properties.
    /// </summary>
    /// <param name="component">A component to remove routing from.</param>
    protected void RemoveRoutePropertyChanged(IObservableHost component) {
        if (_propertyRouters?.TryGetValue(component, out var router) ?? false) {
            router.Deinit();
            _propertyRouters.Remove(component);

            if (router.Prefix is { } prefix) {
                _propertyRoutes!.Remove(prefix);
            }
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
        // Properties with routes (e.g. Image.Sprite called on a Background) won't be added to the main host
        if (_propertyRoutes?.Count > 0) {
            var path = propertyName.Split('.');

            if (path.Length > 0 && _propertyRoutes.TryGetValue(path[0], out var host)) {
                // Remove the first part of the path as it defines the route
                propertyName = string.Concat(path.Skip(1));

                host.AddCallback(propertyName, callback);
                return;
            }
        }

        _observableHost.AddCallback(propertyName, callback);
    }

    public void RemoveCallback<T>(string propertyName, Action<T> callback) {
        _observableHost.RemoveCallback(propertyName, callback);
    }

    #endregion
}