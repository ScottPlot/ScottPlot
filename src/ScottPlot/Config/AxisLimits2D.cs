using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Config
{
    public class AxisLimits2D
    {
        // TODO: add methods to make it easy to compare limits to determine if they are equal (useful for testing)

        public double x1 { get; private set; }
        public double x2 { get; private set; }
        public double y1 { get; private set; }
        public double y2 { get; private set; }

        public double xSpan { get { return x2 - x1; } }
        public double ySpan { get { return y2 - y1; } }

        public AxisLimits2D(double x1, double x2, double y1, double y2)
        {
            this.x1 = x1;
            this.x2 = x2;
            this.y1 = y1;
            this.y2 = y2;
        }

        public AxisLimits2D(double[] limits)
        {
            if (limits == null || limits.Length != 4)
                    throw new ArgumentException();

            x1 = limits[0];
            x2 = limits[1];
            y1 = limits[2];
            y2 = limits[3];
        }

        public override string ToString()
        {
            return $"x1={x1}, x2={x2}, y1={y1}, y2={y2}";
        }

        public void ApplyRounding(int decimals = 2)
        {
            x1 = Math.Round(x1, decimals);
            x2 = Math.Round(x2, decimals);
            y1 = Math.Round(y1, decimals);
            y2 = Math.Round(y2, decimals);
        }
    }
}
