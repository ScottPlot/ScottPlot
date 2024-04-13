using System;
using System.Collections.Generic;
using System.Collections;

namespace ScottPlot
{
    /// <summary>
    /// This object describes the 4 edges of a rectangular view in 2D space.
    /// Values may contain NaN to describe undefined or uninitialized edges.
    /// </summary>
    public readonly struct AxisLimits : IEquatable<AxisLimits>
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

        public static AxisLimits FromRect(CoordinateRect rect)
        {
            return new AxisLimits(rect.XMin, rect.XMax, rect.YMin, rect.YMax);
        }

        public AxisLimits WithX(double xMin, double xMax)
        {
            return new AxisLimits(xMin, xMax, YMin, YMax);
        }

        public AxisLimits WithY(double yMin, double yMax)
        {
            return new AxisLimits(XMin, XMax, yMin, yMax);
        }

        /// <summary>
        /// Return a new set of axis limits panned by the given distance (in axis / coordinate units).
        /// </summary>
        public AxisLimits WithPan(double dX, double dY)
        {
            return new AxisLimits(XMin + dX, XMax + dX, YMin + dY, YMax + dY);
        }

        /// <summary>
        /// Return a new set of axis limits panned by the given fraction.
        /// If <paramref name="xFrac"/> is 0.1 then the returned limits will be shifted 10% to the right.
        /// If <paramref name="yFrac"/> is 0.1 then the returned limits will be shifted 10% upward.
        /// </summary>
        public AxisLimits WithPanFraction(double xFrac, double yFrac)
        {
            return WithPan(XSpan * xFrac, YSpan * yFrac);
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
        /// Return the maximum boundary for this set of axis limits and the given coordinates
        /// </summary>
        public AxisLimits Expand(double x, double y)
        {
            AxisLimits pointLimits = new(x, x, y, y);
            return Expand(pointLimits);
        }

        /// <summary>
        /// Return the maximum boundary for this set of axis limits and the given coordinates
        /// </summary>
        public AxisLimits Expand(Coordinate coordinate)
        {
            return Expand(coordinate.X, coordinate.Y);
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

        public bool Equals(AxisLimits other)
        {
            return other.XMin == XMin &&
                other.XMax == XMax &&
                other.YMin == YMin &&
                other.YMax == YMax;
        }

        public override bool Equals(object obj)
        {
            return obj is AxisLimits && Equals((AxisLimits)obj);
        }

        public static bool operator ==(AxisLimits left, AxisLimits right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(AxisLimits left, AxisLimits right)
        {
            return !Equals(left, right);
        }

        public override int GetHashCode()
        {
            int A = XMin.GetHashCode();
            int B = XMin.GetHashCode();
            int C = XMin.GetHashCode();
            int D = XMin.GetHashCode();
            return A ^ B ^ C ^ D;
        }
    }
}
