using System;

namespace ScottPlot
{
    public struct AxisLimits : IEquatable<AxisLimits>
    {
        public readonly double XMin;
        public readonly double XMax;
        public readonly double YMin;
        public readonly double YMax;
        public override string ToString() => $"AxisLimits: x=[{XMin}, {XMax}] y=[{YMin}, {YMax}]";

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

        // TODO: use this everywhere in the codebase
        /// <summary>
        /// AxisLimits representing uninitialized or "no data" limits (all limits are NaN)
        /// </summary>
        public static AxisLimits NoLimits => new(double.NaN, double.NaN, double.NaN, double.NaN);

        public bool Equals(AxisLimits other) =>
            other.XMin == XMin &&
            other.XMax == XMax &&
            other.YMin == YMin &&
            other.YMax == YMax;
    }
}
