using System;

namespace ScottPlot
{
    /// <summary>
    /// Mutable axis limits for a 2D space
    /// </summary>
    public class AxisLimits2D
    {
        public double XMin { get; set; }
        public double XMax { get; set; }
        public double YMin { get; set; }
        public double YMax { get; set; }

        public override string ToString() => $"[{XMin}, {XMax}, {YMin}, {YMax}]";

        public AxisLimits2D()
        {
            XMin = double.NaN;
            XMax = double.NaN;
            YMin = double.NaN;
            YMax = double.NaN;
        }

        public AxisLimits2D(double? xMin, double? xMax, double? yMin, double? yMax)
        {
            XMin = xMin ?? double.NaN;
            XMax = xMax ?? double.NaN;
            YMin = yMin ?? double.NaN;
            YMax = yMax ?? double.NaN;
        }

        public AxisLimits2D(double[] limits)
        {
            if (limits is null || limits.Length != 4)
                throw new ArgumentException("limits must have 4 values");

            XMin = limits[0];
            XMax = limits[1];
            YMin = limits[2];
            YMax = limits[3];
        }
    }
}
