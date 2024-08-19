using System.Collections.Generic;

namespace Reactive {
    public interface ILayoutDriver {
        IEnumerable<ILayoutItem> Children { get; }
        ILayoutController? LayoutController { get; set; }

        void AppendChild(ILayoutItem comp);
        void TruncateChild(ILayoutItem comp);
        void RecalculateLayoutTree();
        void RecalculateLayout();
    }
}