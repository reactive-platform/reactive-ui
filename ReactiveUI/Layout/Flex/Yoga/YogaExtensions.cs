namespace Reactive.Yoga;

internal static class YogaExtensions {
    public static bool GetIsInitialized(this YogaNode? node) {
        return node is { IsInitialized: true };
    }
}