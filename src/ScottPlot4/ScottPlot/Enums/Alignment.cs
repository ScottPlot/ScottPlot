using System;

namespace ScottPlot
{
    /// <summary>
    /// Vertical (upper/middle/lower) and Horizontal (left/center/right) alignment
    /// </summary>
    public enum Alignment
    {
        UpperLeft,
        UpperCenter,
        UpperRight,
        MiddleLeft,
        MiddleCenter,
        MiddleRight,
        LowerLeft,
        LowerCenter,
        LowerRight,
    }

    [Obsolete("use Alignment", true)]
    public enum TextAlignment { }

    [Obsolete("use Alignment", true)]
    public enum ImageAlignment { }

    [Obsolete("use Alignment", true)]
    public enum legendLocation { }

    [Obsolete("use Alignment", true)]
    public enum shadowDirection { }
}
