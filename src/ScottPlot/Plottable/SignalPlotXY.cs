using ScottPlot.MinMaxSearchStrategies;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// A variation of the SignalPlot optimized for unevenly-spaced ascending X values.
    /// </summary>
    public class SignalPlotXY : SignalPlotXYGeneric<double, double>
    {

        public SignalPlotXY() : base()
        {
            Strategy = new LinearDoubleOnlyMinMaxStrategy();
        }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.Label) ? "" : $" ({this.Label})";
            return $"PlottableSignalXY{label} with {PointCount} points";
        }
    }
}
