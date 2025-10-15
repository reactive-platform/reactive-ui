using JetBrains.Annotations;

namespace Reactive;

/// <summary>
/// Represents an object that can cause layout recalculation. It is recommended to use these methods
/// only in case you know what you are doing (e.g. when you need to recalculate a disabled object).
/// </summary>
[PublicAPI]
public interface ILayoutRecalculationSource {
    /// <summary>
    /// Tells the component to perform an immediate layout recalculation.
    /// </summary>
    void RecalculateLayoutImmediate();
        
    /// <summary>
    /// Tells the component to schedule layout recalculation to the end of this frame.
    /// </summary>
    void ScheduleLayoutRecalculation();
}