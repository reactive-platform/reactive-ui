using System.Collections.Generic;
using JetBrains.Annotations;

namespace Reactive;

/// <summary>
/// An equality comparer for the <see cref="ILayoutItem"/> interface. You must use this one instead
/// of the default one with dictionaries and sets. Breaking this rule may lead to
/// undefined behaviour and even application crash without any info in log.
/// </summary>
[PublicAPI]
public class LayoutEqualityComparer : IEqualityComparer<ILayoutItem> {
    public bool Equals(ILayoutItem? x, ILayoutItem? y) {
        if (x == null || y == null) {
            return false;
        }

        return x.EqualsToLayoutItem(y);
    }

    public int GetHashCode(ILayoutItem obj) {
        return obj.GetLayoutItemHashCode();
    }
}