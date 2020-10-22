using ScottPlot.MinMaxSearchStrategies;

namespace ScottPlot
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
            return $"PlottableSignal{label} with {GetPointCount()} points";
        }
    }
}
