using ScottPlot.MinMaxSearchStrategies;
using System.Drawing;

namespace ScottPlot
{
    public class PlottableSignalXY : PlottableSignalXYGeneric<double, double>
    {
        public PlottableSignalXY(double[] xs, double[] ys, Color color, double lineWidth, double markerSize, string label, int minRenderIndex, int maxRenderIndex, LineStyle lineStyle, bool useParallel)
            : base(xs, ys, color, lineWidth, markerSize, label, minRenderIndex, maxRenderIndex, lineStyle, useParallel, new LinearDoubleOnlyMinMaxStrategy())
        {
        }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.Label) ? "" : $" ({this.Label})";
            return $"PlottableSignalXY{label} with {GetPointCount()} points";
        }
    }
}
