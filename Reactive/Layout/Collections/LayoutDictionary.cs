using System.Collections.Generic;
using JetBrains.Annotations;

namespace Reactive;

/// <summary>
/// Default <see cref="Dictionary{TValue, TKey}"/> replacement for working with <see cref="ILayoutItem"/>.
/// You must use this one instead of the default one. Breaking this rule may lead to
/// undefined behaviour and even application crash without any info in log.
/// </summary>
[PublicAPI]
public class LayoutDictionary<TValue> : Dictionary<ILayoutItem, TValue> {
    public LayoutDictionary() : base(new LayoutEqualityComparer()) { }

    public LayoutDictionary(int capacity) : base(capacity, new LayoutEqualityComparer()) { }

    public LayoutDictionary(IDictionary<ILayoutItem, TValue> dictionary) : base(dictionary, new LayoutEqualityComparer()) { }
}