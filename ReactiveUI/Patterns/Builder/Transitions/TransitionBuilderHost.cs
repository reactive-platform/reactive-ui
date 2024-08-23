using System;
using System.Linq.Expressions;

namespace Reactive;

public class TransitionBuilderHost<T, TValue>(
    T target,
    Expression<Func<T, TValue>> expression,
    TransitionBuilder<TValue> builder
) : ITransitionBuilder {
    public ComponentState BuildingForState => builder.State;

    public IAnimation Build() {
        return builder.Build(target, expression);
    }
}