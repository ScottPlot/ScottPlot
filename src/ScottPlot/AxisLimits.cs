using System;

namespace ScottPlot
{
    public class AxisLimits
    {
        public double XMin { get; set; }
        public double XMax { get; set; }
        public double YMin { get; set; }
        public double YMax { get; set; }

        public double[] AsArray => new double[] { XMin, XMax, YMin, YMax };
        public (double xMin, double xMax, double yMin, double yMax) AsTuple => (XMin, XMax, YMin, YMax);

        public override string ToString() => $"Axis limits: X=[{XMin}, {XMax}] Y=[{YMin}, {YMax}]";

        public AxisLimits() =>
            (XMin, XMax, YMin, YMax) = (double.NaN, double.NaN, double.NaN, double.NaN);

        public AxisLimits(double xMin, double xMax, double yMin, double yMax) =>
            (XMin, XMax, YMin, YMax) = (xMin, xMax, yMin, yMax);

        public AxisLimits(double[] limits)
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
