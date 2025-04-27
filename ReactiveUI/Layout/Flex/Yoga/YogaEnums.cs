using JetBrains.Annotations;

namespace Reactive.Yoga {
    /// <summary>
    /// Defines how items are aligned along the cross axis in a flex container.
    /// </summary>
    [PublicAPI]
    public enum Align {
        /// <summary>Default value. Items inherit the parent's alignment.</summary>
        Auto,
        /// <summary>Items are aligned to the start of the cross axis.</summary>
        FlexStart,
        /// <summary>Items are centered along the cross axis.</summary>
        Center,
        /// <summary>Items are aligned to the end of the cross axis.</summary>
        FlexEnd,
        /// <summary>Items stretch to fill the container along the cross axis.</summary>
        Stretch,
        /// <summary>Items are aligned to their text baseline.</summary>
        Baseline,
        /// <summary>Items are evenly distributed with space between them.</summary>
        SpaceBetween,
        /// <summary>Items are evenly distributed with equal space around them.</summary>
        SpaceAround,
        /// <summary>Items are evenly distributed with equal space between and around them.</summary>
        SpaceEvenly,
    }

    /// <summary>
    /// Defines the layout direction.
    /// </summary>
    [PublicAPI]
    public enum Direction {
        /// <summary>Direction is inherited from the parent element.</summary>
        Inherit,
        /// <summary>Text and layout flow from left to right.</summary>
        LeftToRight,
        /// <summary>Text and layout flow from right to left.</summary>
        RightToLeft,
    }

    /// <summary>
    /// Defines how an element is displayed.
    /// </summary>
    [PublicAPI]
    public enum Display {
        /// <summary>Element behaves as a flex container.</summary>
        Flex,
        /// <summary>Element is not displayed.</summary>
        None,
        /// <summary>Element behaves as if it were not in the layout tree.</summary>
        Contents
    }

    /// <summary>
    /// Defines which edge of an element to position or apply spacing to.
    /// </summary>
    [PublicAPI]
    internal enum Edge {
        /// <summary>Left edge of the element.</summary>
        Left,
        /// <summary>Top edge of the element.</summary>
        Top,
        /// <summary>Right edge of the element.</summary>
        Right,
        /// <summary>Bottom edge of the element.</summary>
        Bottom,
        /// <summary>Start edge (left in LTR, right in RTL).</summary>
        Start,
        /// <summary>End edge (right in LTR, left in RTL).</summary>
        End,
        /// <summary>Both left and right edges.</summary>
        Horizontal,
        /// <summary>Both top and bottom edges.</summary>
        Vertical,
        /// <summary>All edges.</summary>
        All,
    }

    /// <summary>
    /// Defines the direction of the main axis in a flex container.
    /// </summary>
    [PublicAPI]
    public enum FlexDirection {
        /// <summary>Items are stacked vertically from top to bottom.</summary>
        Column,
        /// <summary>Items are stacked vertically from bottom to top.</summary>
        ColumnReverse,
        /// <summary>Items are arranged horizontally from left to right.</summary>
        Row,
        /// <summary>Items are arranged horizontally from right to left.</summary>
        RowReverse,
    }

    /// <summary>
    /// Defines how items are distributed along the main axis.
    /// </summary>
    [PublicAPI]
    public enum Justify {
        /// <summary>Items are packed toward the start of the main axis.</summary>
        FlexStart,
        /// <summary>Items are centered along the main axis.</summary>
        Center,
        /// <summary>Items are packed toward the end of the main axis.</summary>
        FlexEnd,
        /// <summary>Items are evenly distributed with space between them.</summary>
        SpaceBetween,
        /// <summary>Items are evenly distributed with equal space around them.</summary>
        SpaceAround,
        /// <summary>Items are evenly distributed with equal space between and around them.</summary>
        SpaceEvenly,
    }

    /// <summary>
    /// Defines which axis to apply gutters (spacing) to.
    /// </summary>
    [PublicAPI]
    public enum Gutter {
        /// <summary>Apply gutters to columns.</summary>
        Column,
        /// <summary>Apply gutters to rows.</summary>
        Row,
        /// <summary>Apply gutters to both columns and rows.</summary>
        All
    }

    /// <summary>
    /// Defines how content that overflows an element's box is handled.
    /// </summary>
    [PublicAPI]
    public enum Overflow {
        /// <summary>Content is not clipped and may be visible outside the element's box.</summary>
        Visible,
        /// <summary>Content is clipped and not accessible.</summary>
        Hidden
        // Scroll is omitted since yoga has it just for compatibility
    }

    /// <summary>
    /// Defines how an element is positioned in the layout.
    /// </summary>
    [PublicAPI]
    public enum PositionType {
        /// <summary>Element is positioned according to the normal flow of the layout.</summary>
        Static,
        /// <summary>Element is positioned relative to its normal position.</summary>
        Relative,
        /// <summary>Element is positioned relative to its nearest positioned ancestor.</summary>
        Absolute,
    }

    /// <summary>
    /// Defines the unit of measurement for dimensions and positions.
    /// </summary>
    [PublicAPI]
    public enum Unit {
        /// <summary>No unit specified.</summary>
        Undefined,
        /// <summary>Measurement in points.</summary>
        Point,
        /// <summary>Measurement as a percentage.</summary>
        Percent,
        /// <summary>Automatic sizing.</summary>
        Auto,
        /// <summary>Sized to fit maximum content.</summary>
        MaxContent,
        /// <summary>Sized to fit content.</summary>
        FitContent,
        /// <summary>Stretched to fill available space.</summary>
        Stretch
    }

    /// <summary>
    /// Defines how flex items wrap when they exceed the container's size.
    /// </summary>
    [PublicAPI]
    public enum Wrap {
        /// <summary>Items do not wrap to the next line.</summary>
        NoWrap,
        /// <summary>Items wrap to the next line when needed.</summary>
        Wrap,
        /// <summary>Items wrap to the previous line when needed.</summary>
        WrapReverse,
    }

    /// <summary>
    /// Defines how an element's size is measured.
    /// </summary>
    [PublicAPI]
    public enum MeasureMode {
        /// <summary>No specific measurement mode.</summary>
        Undefined,
        /// <summary>Element must be exactly the specified size.</summary>
        Exactly,
        /// <summary>Element can be up to the specified size.</summary>
        AtMost
    }

    /// <summary>
    /// Defines the severity level of log messages.
    /// </summary>
    [PublicAPI]
    public enum LogLevel {
        /// <summary>Error level messages for critical issues.</summary>
        Error,
        /// <summary>Warning level messages for potential issues.</summary>
        Warn,
        /// <summary>Information level messages for general updates.</summary>
        Info,
        /// <summary>Debug level messages for development troubleshooting.</summary>
        Debug,
        /// <summary>Verbose level messages for detailed information.</summary>
        Verbose,
        /// <summary>Fatal level messages for unrecoverable errors.</summary>
        Fatal
    }
}