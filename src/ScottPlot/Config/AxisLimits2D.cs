using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Config
{
    public class AxisLimits2D
    {
        public double x1 { get; private set; }
        public double x2 { get; private set; }
        public double y1 { get; private set; }
        public double y2 { get; private set; }
        public double xSpan => x2 - x1;
        public double ySpan => y2 - y1;

        public AxisLimits2D()
        {
            x1 = double.NaN;
            x2 = double.NaN;
            y1 = double.NaN;
            y2 = double.NaN;
        }

        public AxisLimits2D(double? xMin, double? xMax, double? yMin, double? yMax)
        {
            x1 = xMin ?? double.NaN;
            x2 = xMax ?? double.NaN;
            y1 = yMin ?? double.NaN;
            y2 = yMax ?? double.NaN;
        }

        public AxisLimits2D(double[] limits)
        {
            if (limits is null || limits.Length != 4)
                throw new ArgumentException("limits must have 4 values");

            x1 = limits[0];
            x2 = limits[1];
            y1 = limits[2];
            y2 = limits[3];
        }

        public override string ToString() => $"[{x1}, {x2}, {y1}, {y2}]";

        public void Set(double? xMin, double? xMax, double? yMin, double? yMax)
        {
            if (xMin.HasValue && !double.IsNaN(xMin.Value)) x1 = xMin.Value;
            if (xMax.HasValue && !double.IsNaN(xMax.Value)) x2 = xMax.Value;
            if (yMin.HasValue && !double.IsNaN(yMin.Value)) y1 = yMin.Value;
            if (yMax.HasValue && !double.IsNaN(yMax.Value)) y2 = yMax.Value;
        }

        public void Expand(double? xMin, double? xMax, double? yMin, double? yMax)
        {
            if (xMin.HasValue && !double.IsNaN(xMin.Value) && double.IsNaN(x1)) x1 = xMin.Value;
            if (xMax.HasValue && !double.IsNaN(xMax.Value) && double.IsNaN(x2)) x2 = xMax.Value;
            if (yMin.HasValue && !double.IsNaN(yMin.Value) && double.IsNaN(y1)) y1 = yMin.Value;
            if (yMax.HasValue && !double.IsNaN(yMax.Value) && double.IsNaN(y2)) y2 = yMax.Value;

            if (xMin.HasValue && !double.IsNaN(xMin.Value)) x1 = Math.Min(x1, xMin.Value);
            if (xMax.HasValue && !double.IsNaN(xMax.Value)) x2 = Math.Max(x2, xMax.Value);
            if (yMin.HasValue && !double.IsNaN(yMin.Value)) y1 = Math.Min(y1, yMin.Value);
            if (yMax.HasValue && !double.IsNaN(yMax.Value)) y2 = Math.Max(y2, yMax.Value);
        }
    }
}
