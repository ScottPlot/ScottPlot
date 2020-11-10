using System;

namespace ScottPlot.Drawing
{
    /// <summary>
    /// A DTO to make it easy to pass a set of 2D axis limits
    /// </summary>
    public class AxisLimits2D
    {
        public double XMin { get; private set; }
        public double XMax { get; private set; }
        public double YMin { get; private set; }
        public double YMax { get; private set; }

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

        public override string ToString() => $"[{XMin}, {XMax}, {YMin}, {YMax}]";

        public void Set(double? xMin, double? xMax, double? yMin, double? yMax)
        {
            if (xMin.HasValue && !double.IsNaN(xMin.Value)) XMin = xMin.Value;
            if (xMax.HasValue && !double.IsNaN(xMax.Value)) XMax = xMax.Value;
            if (yMin.HasValue && !double.IsNaN(yMin.Value)) YMin = yMin.Value;
            if (yMax.HasValue && !double.IsNaN(yMax.Value)) YMax = yMax.Value;
        }

        public void Expand(double? xMin, double? xMax, double? yMin, double? yMax)
        {
            if (xMin.HasValue && !double.IsNaN(xMin.Value) && double.IsNaN(XMin)) XMin = xMin.Value;
            if (xMax.HasValue && !double.IsNaN(xMax.Value) && double.IsNaN(XMax)) XMax = xMax.Value;
            if (yMin.HasValue && !double.IsNaN(yMin.Value) && double.IsNaN(YMin)) YMin = yMin.Value;
            if (yMax.HasValue && !double.IsNaN(yMax.Value) && double.IsNaN(YMax)) YMax = yMax.Value;

            if (xMin.HasValue && !double.IsNaN(xMin.Value)) XMin = Math.Min(XMin, xMin.Value);
            if (xMax.HasValue && !double.IsNaN(xMax.Value)) XMax = Math.Max(XMax, xMax.Value);
            if (yMin.HasValue && !double.IsNaN(yMin.Value)) YMin = Math.Min(YMin, yMin.Value);
            if (yMax.HasValue && !double.IsNaN(yMax.Value)) YMax = Math.Max(YMax, yMax.Value);
        }
    }
}
