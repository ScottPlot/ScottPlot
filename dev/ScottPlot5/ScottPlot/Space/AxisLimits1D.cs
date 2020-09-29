using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Space
{
    public class AxisLimits1D
    {
        public double Min = double.NaN;
        public double Max = double.NaN;
        public bool IsValid { get => double.IsNaN(Min) == false && double.IsNaN(Max) == false && Max > Min; }

        public AxisLimits1D() { }

        public AxisLimits1D(double min, double max) => (Min, Max) = (min, max);

        public override string ToString() => $"[{Min}, {Max}]";

        public void Expand(AxisLimits1D newLimits)
        {
            Min = double.IsNaN(Min) ? newLimits.Min : Math.Min(Min, newLimits.Min);
            Min = double.IsNaN(Max) ? newLimits.Max : Math.Max(Max, newLimits.Max);
        }
    }
}
