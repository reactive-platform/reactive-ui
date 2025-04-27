using System.Collections.Generic;
using JetBrains.Annotations;

namespace Reactive {
    /// <summary>
    /// Represents an object that contains children. The driver itself does not control the children,
    /// but provides all necessary data to the layout controller if it's presented.
    /// </summary>
    [PublicAPI]
    public interface ILayoutDriver : ILayoutRecalculationSource {
        /// <summary>
        /// An observable collection with children.
        /// </summary>
        ICollection<ILayoutItem> Children { get; }
        ILayoutController? LayoutController { get; set; }
    }
}