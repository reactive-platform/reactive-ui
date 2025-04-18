namespace Reactive.Yoga {
    public enum Align {
        Auto,
        FlexStart,
        Center,
        FlexEnd,
        Stretch,
        Baseline,
        SpaceBetween,
        SpaceAround,
        SpaceEvenly,
    }

    public enum Direction {
        Inherit,
        LeftToRight,
        RightToLeft,
    }

    public enum Display {
        Flex,
        None,
    }

    public enum Edge {
        Left,
        Top,
        Right,
        Bottom,
        Start,
        End,
        Horizontal,
        Vertical,
        All,
    }

    public enum FlexDirection {
        Column,
        ColumnReverse,
        Row,
        RowReverse,
    }

    public enum Justify {
        FlexStart,
        Center,
        FlexEnd,
        SpaceBetween,
        SpaceAround,
        SpaceEvenly,
    }

    public enum Gutter {
        Column,
        Row,
        All
    }

    public enum Overflow {
        Visible,
        Hidden,
        Scroll,
    }

    public enum PositionType {
        Static,
        Relative,
        Absolute,
    }

    public enum PrintOptions {
        Layout = 1,
        Style = 2,
        Children = 4,
    }

    public enum Unit {
        Undefined,
        Point,
        Percent,
        Auto,
    }

    public enum Wrap {
        NoWrap,
        Wrap,
        WrapReverse,
    }

    /// <summary>
    /// The modes available to measure element sizes.
    /// </summary>
    public enum MeasureMode {
        /// <summary>
        /// The parent has not imposed any constraint on the child. It can be whatever size it wants.
        /// </summary>
        Undefined,

        /// <summary>
        /// The child can be as large as it wants up to the specified size.
        /// </summary>
        Exactly,

        /// <summary>
        /// The parent has determined an exact size for the child. The child is going to be given those bounds regardless of how big it wants to be.
        /// </summary>
        AtMost
    }

    public enum LogLevel {
        Error,
        Warn,
        Info,
        Debug,
        Verbose,
        Fatal
    }
}