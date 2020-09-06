using System;

namespace ScottPlot.Space
{
    // TODO: delete this class and only interact with 1D limits
    public class AxisLimits2D
    {
        public AxisLimits1D X;
        public AxisLimits1D Y;

        public double X1 { get => X.Min; set => X.Min = value; }
        public double X2 { get => X.Max; set => X.Max = value; }
        public double Y1 { get => Y.Min; set => Y.Min = value; }
        public double Y2 { get => Y.Max; set => Y.Max = value; }
        public bool IsValid { get { return X.IsValid && Y.IsValid; } }

        public AxisLimits2D()
        {
            X = new AxisLimits1D();
            Y = new AxisLimits1D();
        }

        public AxisLimits2D(double xMin, double xMax, double yMin, double yMax)
        {
            X = new AxisLimits1D(xMin, xMax);
            Y = new AxisLimits1D(yMin, yMax);
        }

        public override string ToString() => $"AxisLimits X={X} Y={Y}";

        public void Expand(AxisLimits2D newLimits)
        {
            X.Expand(newLimits.X);
            Y.Expand(newLimits.Y);
        }
    }
}
