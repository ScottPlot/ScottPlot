using System;
using System.Collections.Generic;
using System.Linq;

namespace ScottPlot
{
    /// <summary>
    /// Describes an X/Y position in coordinate space
    /// </summary>
    public struct Coordinate
    {
        public double X;
        public double Y;

        public Coordinate(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override string ToString() => $"(X={X}, Y={Y})";

        /// <summary>
        /// True as lone as neither coordinate is NaN or Infinity
        /// </summary>
        public bool IsFinite()
        {
            return
                !double.IsNaN(X) &&
                !double.IsNaN(Y) &&
                !double.IsInfinity(X) &&
                !double.IsInfinity(Y);
        }

        /// <summary>
        /// Return the distance to another coordinate (in coordinate units)
        /// </summary>
        public double Distance(Coordinate other)
        {
            double dX = Math.Abs(other.X - X);
            double dY = Math.Abs(other.Y - Y);
            return Math.Sqrt(dX * dX + dY * dY);
        }

        public static Coordinate FromGeneric<T>(T x, T y)
        {
            return new(NumericConversion.GenericToDouble(ref x), NumericConversion.GenericToDouble(ref y));
        }

        public Pixel ToPixel(PlotDimensions dims)
        {
            return dims.GetPixel(this);
        }
    }

    public static class CoordinateExtensions
    {
        public static AxisLimits GetLimits(this IEnumerable<Coordinate> coordinates)
        {
            if (!coordinates.Any())
                return AxisLimits.NoLimits;

            double xMin = coordinates.First().X;
            double xMax = coordinates.First().X;
            double yMin = coordinates.First().Y;
            double yMax = coordinates.First().Y;

            foreach (Coordinate coord in coordinates)
            {
                xMin = Math.Min(coord.X, xMin);
                xMax = Math.Max(coord.X, xMax);
                yMin = Math.Min(coord.Y, yMin);
                yMax = Math.Max(coord.Y, yMax);
            }

            return new AxisLimits(xMin, xMax, yMin, yMax);
        }
    }
}
