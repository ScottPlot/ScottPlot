using System.Drawing;
using ScottPlot.MinMaxSearchStrategies;

namespace ScottPlot
{
    public class PlottableSignal : PlottableSignalBase<double>
    {
        public PlottableSignal(double[] ys, double sampleRate, double xOffset, double yOffset, Color color,
            double lineWidth, double markerSize, string label, Color[] colorByDensity,
            int minRenderIndex, int maxRenderIndex, LineStyle lineStyle, bool useParallel, bool fill,
            Color? fillColor1, Color? fillColor2)
            : base(ys, sampleRate, xOffset, yOffset, color, lineWidth, markerSize, label, colorByDensity,
                 minRenderIndex, maxRenderIndex, lineStyle, useParallel, new LinearDoubleOnlyMinMaxStrategy(),
                 fill, fillColor1, fillColor2)
        {
        }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.label) ? "" : $" ({this.label})";
            return $"PlottableSignal{label} with {GetPointCount()} points";
        }
    }
}
