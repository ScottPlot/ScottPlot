using System;

namespace ScottPlot
{
    /// <summary>
    /// Vertical (upper/middle/lower) and Horizontal (left/center/right) alignment
    /// </summary>
    public enum Alignment
    {
        UpperLeft,
        UpperRight,
        UpperCenter,
        MiddleLeft,
        MiddleCenter,
        MiddleRight,
        LowerLeft,
        LowerRight,
        LowerCenter
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
