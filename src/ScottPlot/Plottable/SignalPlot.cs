using ScottPlot.MinMaxSearchStrategies;

namespace ScottPlot.Plottable
{
    public class SignalPlot : SignalPlotBase<double>
    {
        public SignalPlot() : base()
        {
            minmaxSearchStrategy = new LinearDoubleOnlyMinMaxStrategy();
        }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.label) ? "" : $" ({this.label})";
            return $"PlottableSignal{label} with {PointCount} points";
        }
    }
}
