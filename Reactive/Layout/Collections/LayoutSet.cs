using System.Collections.Generic;
using JetBrains.Annotations;

namespace Reactive;

/// <summary>
/// Default <see cref="HashSet{T}"/> replacement for working with <see cref="ILayoutItem"/>.
/// You must use this one instead of the default one. Breaking this rule may lead to
/// undefined behaviour and even application crash without any info in log.
/// </summary>
[PublicAPI]
public class LayoutSet : HashSet<ILayoutItem> {
    public LayoutSet() : base(new LayoutEqualityComparer()) { }

    public LayoutSet(int capacity) : base(capacity, new LayoutEqualityComparer()) { }
    
    public LayoutSet(IEnumerable<ILayoutItem> collection) : base(collection, new LayoutEqualityComparer()) { }
}