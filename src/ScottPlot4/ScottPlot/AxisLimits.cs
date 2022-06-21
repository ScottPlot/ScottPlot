using System;

namespace ScottPlot
{
    /// <summary>
    /// This object describes the 4 edges of a rectangular view in 2D space.
    /// Values may contain NaN to describe undefined or uninitialized edges.
    /// </summary>
    public struct AxisLimits : IEquatable<AxisLimits>
    {
        public readonly double XMin;
        public readonly double XMax;
        public readonly double YMin;
        public readonly double YMax;

        public readonly double XSpan;
        public readonly double YSpan;
        public readonly double XCenter;
        public readonly double YCenter;

        public AxisLimits(double xMin, double xMax, double yMin, double yMax)
        {
            (XMin, XMax, YMin, YMax) = (xMin, xMax, yMin, yMax);
            (XSpan, YSpan) = (XMax - XMin, YMax - YMin);
            (XCenter, YCenter) = (XMin + XSpan / 2, YMin + YSpan / 2);
        }

        public override string ToString()
        {
            return $"AxisLimits: x=[{XMin}, {XMax}] y=[{YMin}, {YMax}]";
        }

        /// <summary>
        /// AxisLimits representing uninitialized or "no data" limits (all limits are NaN)
        /// </summary>
        public static AxisLimits NoLimits => new(double.NaN, double.NaN, double.NaN, double.NaN);

        /// <summary>
        /// AxisLimits with finite vertical limits and undefined (NaN) horizontal limits
        /// </summary>
        public static AxisLimits VerticalLimitsOnly(double yMin, double yMax) => new(double.NaN, double.NaN, yMin, yMax);

        /// <summary>
        /// AxisLimits with finite horizontal limits and undefined (NaN) vertical limits
        /// </summary>
        public static AxisLimits HorizontalLimitsOnly(double xMin, double xMax) => new(xMin, xMax, double.NaN, double.NaN);

        /// <summary>
        /// Return the maximum boundary for both sets of axis limits
        /// </summary>
        public AxisLimits Expand(AxisLimits limits)
        {
            return new AxisLimits(
                xMin: double.IsNaN(XMin) ? limits.XMin : Math.Min(XMin, limits.XMin),
                xMax: double.IsNaN(XMax) ? limits.XMax : Math.Max(XMax, limits.XMax),
                yMin: double.IsNaN(YMin) ? limits.YMin : Math.Min(YMin, limits.YMin),
                yMax: double.IsNaN(YMax) ? limits.YMax : Math.Max(YMax, limits.YMax));
        }

        /// <summary>
        /// Returns True if the coordinate is contained inside these axis limits
        /// </summary>
        public bool Contains(Coordinate coordinate)
        {
            return coordinate.X >= XMin
                && coordinate.X <= XMax
                && coordinate.Y >= YMin
                && coordinate.Y <= YMax;
        }

        public bool Equals(AxisLimits other) =>
            other.XMin == XMin &&
            other.XMax == XMax &&
            other.YMin == YMin &&
            other.YMax == YMax;
    }
}
