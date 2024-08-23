using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Reactive {
    [PublicAPI]
    public static class TransitionBuilderExtensions {
        public static T DefineTransition<T, TValue>(
            this T target,
            out TransitionBuilderHost<T, TValue> transition,
            Expression<Func<T, TValue>> expression,
            TransitionBuilder<TValue> builder
        ) {
            transition = new(target, expression, builder);
            return target;
        }
        
        public static T DefineTransitions<T, TValue>(
            this T target,
            out TransitionBuilderHost<T, TValue>[] collection,
            Expression<Func<T, TValue>> expression,
            params TransitionBuilder<TValue>[] builders
        ) {
            collection = builders.Select(x => new TransitionBuilderHost<T, TValue>(target, expression, x)).ToArray();
            return target;
        }

        public static T WithTransition<T>(
            this T target,
            ITransitionBuilder builder
        ) where T : IStateAnimationHost {
            target.AddStateAnimation(builder.BuildingForState, builder.Build());
            return target;
        }

        public static T WithTransitions<T>(
            this T target,
            IEnumerable<ITransitionBuilder> transitions
        ) where T : IStateAnimationHost {
            foreach (var transition in transitions) {
                target.WithTransition(transition);
            }
            return target;
        }

        public static TransitionBuilder<T> MakeTransition<T>(this T value) {
            return new TransitionBuilder<T>().WithValue(value);
        }
    }
}