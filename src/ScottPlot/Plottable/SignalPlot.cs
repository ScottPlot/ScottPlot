using ScottPlot.MinMaxSearchStrategies;

namespace ScottPlot.Plottable
{
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
