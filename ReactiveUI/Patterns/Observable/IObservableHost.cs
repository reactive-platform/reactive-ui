using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Reactive {
    [PublicAPI]
    public interface IObservableHost {
        void AddCallback<T>(string propertyName, Action<T> callback);
        void RemoveCallback<T>(string propertyName, Action<T> callback);
        void NotifyPropertyChanged([CallerMemberName] string? name = null);
    }
}