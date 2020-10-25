using ScottPlot.MinMaxSearchStrategies;

namespace ScottPlot.Plottable
{
    public class PlottableSignal : PlottableSignalBase<double>
    {
        public PlottableSignal() : base()
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
