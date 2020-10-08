using ScottPlot.MinMaxSearchStrategies;
using System.Drawing;

namespace ScottPlot
{
    public class PlottableSignal : PlottableSignalBase<double>
    {
        public PlottableSignal(double[] ys, double sampleRate, double xOffset, double yOffset, Color color,
            double lineWidth, double markerSize, string label, Color[] colorByDensity,
            int minRenderIndex, int maxRenderIndex, LineStyle lineStyle, bool useParallel)
            : base(ys, sampleRate, xOffset, yOffset, color, lineWidth, markerSize, label, colorByDensity,
                 minRenderIndex, maxRenderIndex, lineStyle, useParallel, new LinearDoubleOnlyMinMaxStrategy())
        {
        }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.label) ? "" : $" ({this.label})";
            return $"PlottableSignal{label} with {GetPointCount()} points";
        }
    }
}
