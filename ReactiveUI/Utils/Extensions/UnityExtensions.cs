using UnityEngine;

namespace Reactive {
    public static class UnityExtensions {
        public static T GetOrAddComponent<T>(this GameObject go) where T : Component {
            return go.GetComponent<T>() ?? go.AddComponent<T>();
        }
    }
}