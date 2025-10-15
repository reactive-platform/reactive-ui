using System.Collections.Generic;
using JetBrains.Annotations;

namespace Reactive {
    [PublicAPI]
    public interface IValueInterpolator<T> : IEqualityComparer<T> {
        T Lerp(T from, T to, float t);
    }
}