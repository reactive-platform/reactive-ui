using System;

namespace Reactive {
    public interface ILayoutModifier : ICopiable<ILayoutModifier>, IContextMember {
        event Action? ModifierUpdatedEvent;

        void ReloadLayoutItem(ILayoutItem? item);
    }
}