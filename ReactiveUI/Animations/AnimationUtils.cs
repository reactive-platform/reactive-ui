using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Reactive;

[PublicAPI]
public static class AnimationUtils {
    public static ISharedAnimation Animation(Action onStart, IEnumerable<ISharedAnimation> waitFor) {
        return new SequentialAnimation(onStart, waitFor);
    }
}