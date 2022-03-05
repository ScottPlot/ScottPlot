using ScottPlot.MinMaxSearchStrategies;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// A signal plot displays evenly-spaced data.
    /// Instead of X/Y pairs, signal plots take Y values and a sample rate.
    /// Optional X and Y offsets can further customize the data.
    /// </summary>
    public class SignalPlot : SignalPlotBase<double>
    {
        public SignalPlot() : base()
        {
            Strategy = new LinearDoubleOnlyMinMaxStrategy();
        }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.Label) ? "" : $" ({this.Label})";
            return $"PlottableSignal{label} with {PointCount} points";
        }
    }
}
