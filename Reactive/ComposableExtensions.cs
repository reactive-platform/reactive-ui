using System;
using System.Collections;
using JetBrains.Annotations;

namespace Reactive;

/// <summary>
/// Provides general extensions for building composable UIs.
/// </summary>
[PublicAPI]
public static class ComposableExtensions {
    extension<T>(T comp) where T : IReactiveComponent {
        /// <summary>
        /// Calls provided callback immediately.
        /// </summary>
        public Action<T> Do {
            set => value(comp);
        }

        /// <summary>
        /// Calls all provided callbacks immediately. This property accepts array
        /// instead of <see cref="IEnumerator"/> so compiler can optimize the allocations out.
        /// </summary>
        public Action<T>[] DoAll {
            set {
                foreach (var item in value) {
                    item(comp);
                }
            }
        }
    }
}