using System;
using System.Linq;

// this file contains information about plot objects and their styling.
// this file does NOT contain information about accumulated objects

namespace ScottPlot
{
    public static class Plottables
    {

        public class PlottableThing
        {
            public virtual double[] AxisLimits() { return null; }
            public Style style;
            public int pointCount = 1;
        }

        public class Scatter : PlottableThing
        {
            public readonly double[] xs, ys;

            public Scatter(double[] xs, double[] ys, Style style)
            {
                if (ys == null || xs == null)
                    throw new Exception("scatter data cannot be null");
                if (xs.Length != ys.Length)
                    throw new Exception("scatter plots must have the same number of X and Y points");

                this.xs = xs;
                this.ys = ys;
                this.style = style;
                pointCount = ys.Length;
            }

            public override string ToString() { return $"scatter data with {ys.Length} X/Y pairs"; }
            public override double[] AxisLimits() { return new double[] { xs.Min(), xs.Max(), ys.Min(), ys.Max() }; }

        }

        public class Signal : PlottableThing
        {
            public readonly double[] ys;
            public readonly double sampleRateHz;

            public Signal(double[] ys, double sampleRateHz, Style style)
            {
                this.ys = ys;
                this.sampleRateHz = sampleRateHz;
                this.style = style;
                pointCount = ys.Length;
            }

            public override string ToString() { return $"signal data with {ys.Length} points at {sampleRateHz} Hz"; }
            public override double[] AxisLimits() { return new double[] { 0, ys.Length / sampleRateHz, ys.Min(), ys.Max() }; }
        }

        public class AxLine : PlottableThing
        {
            public double position;
            public bool vertical;
            private string orientation;

            public AxLine(double position, bool vertical, Style style)
            {
                this.position = position;
                this.vertical = vertical;
                this.style = style;
                orientation = (vertical) ? "vertical" : "horizontal";
            }

            public override string ToString() { return $"{orientation} line at {position}"; }
        }
    }
}
