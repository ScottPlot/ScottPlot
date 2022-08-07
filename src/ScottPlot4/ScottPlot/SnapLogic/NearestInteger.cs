using System;
using ScottPlot.Plottable;

namespace ScottPlot.SnapLogic
{
    public class NearestInteger : ISnap
    {
        public double Snap(double value)
        {
            return Math.Round(value);
        }
    }
}
