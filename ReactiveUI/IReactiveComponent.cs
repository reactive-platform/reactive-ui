using JetBrains.Annotations;
using UnityEngine;

namespace Reactive {
    [PublicAPI]
    public interface IReactiveComponent {
        GameObject Content { get; }
        RectTransform ContentTransform { get; }

        bool IsDestroyed { get; }
        bool IsInitialized { get; }
        bool Enabled { get; set; }

        GameObject Use(Transform? parent);
    }
}