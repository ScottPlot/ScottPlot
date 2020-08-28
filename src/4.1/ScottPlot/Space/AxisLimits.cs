using System;

namespace ScottPlot.Space
{
    public class AxisLimits
    {
        public double X1 = double.NaN;
        public double X2 = double.NaN;
        public double Y1 = double.NaN;
        public double Y2 = double.NaN;
        public bool IsValidX { get { return double.IsNaN(X1) == false && double.IsNaN(X2) == false && X2 > X1; } }
        public bool IsValidY { get { return double.IsNaN(Y1) == false && double.IsNaN(Y2) == false && Y2 > Y1; } }
        public bool IsValid { get { return IsValidX && IsValidY; } }

        public AxisLimits() { }

        public AxisLimits(double xMin, double xMax, double yMin, double yMax)
        {
            X1 = xMin;
            X2 = xMax;
            Y1 = yMin;
            Y2 = yMax;
        }

        public override string ToString()
        {
            return $"AxisLimits X=[{X1},{X2}] Y=[{Y1},{Y2}]";
        }

        public void Expand(AxisLimits newLimits)
        {
            X1 = double.IsNaN(X1) ? newLimits.X1 : Math.Min(X1, newLimits.X1);
            X2 = double.IsNaN(X2) ? newLimits.X2 : Math.Max(X2, newLimits.X2);
            Y1 = double.IsNaN(Y1) ? newLimits.Y1 : Math.Min(Y1, newLimits.Y1);
            Y2 = double.IsNaN(Y2) ? newLimits.Y2 : Math.Max(Y2, newLimits.Y2);
        }
    }
}
