using JetBrains.Annotations;

namespace Reactive {
    /// <summary>
    /// Represents an object that contains children. The driver itself does not control the children,
    /// but provides all necessary data to the layout controller if it's presented.
    /// </summary>
    [PublicAPI]
    public interface ILayoutDriver {
        ILayoutController? LayoutController { get; set; }

        void AppendChild(ILayoutItem comp);
        void TruncateChild(ILayoutItem comp);
        void RecalculateLayout();
    }
}