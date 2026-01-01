using JetBrains.Annotations;
using Reactive.Yoga;

namespace Reactive;

/// <summary>
/// Provides a set of context-specific default values for components.
/// Works only with managed objects as structures require boxing.
/// </summary>
[PublicAPI]
public static class ComponentDefaults {
    public static readonly ContextProp<ILayoutModifier> LayoutModifier = new() {
        Factory = () => new YogaModifier()
    };

    public static readonly ContextProp<ILayoutController> LayoutController = new() {
        Factory = () => new YogaLayoutController()
    };
}