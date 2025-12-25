using JetBrains.Annotations;
using Reactive.Yoga;

namespace Reactive;

/// <summary>
/// Provides layout extensions for building composable UIs.
/// </summary>
[PublicAPI]
public static class ComposableLayoutExtensions {
    extension(ILayoutItem item) {
        /// <summary>
        /// Returns an existing layout modifier or creates a new one.
        /// </summary>
        public YogaModifier FlexItem {
            get {
                if (item.LayoutModifier is not YogaModifier modifier) {
                    modifier = new();
                    item.LayoutModifier = modifier;
                }

                return modifier;
            }
        }
    }
        
    extension(ILayoutDriver item) {
        /// <summary>
        /// Returns an existing layout controller or creates a new one.
        /// </summary>
        public YogaLayoutController FlexController {
            get {
                if (item.LayoutController is not YogaLayoutController controller) {
                    controller = new();
                    item.LayoutController = controller;
                }

                return controller;
            }
        }
    }

}