using ScottPlot.MinMaxSearchStrategies;

namespace ScottPlot
{
    public class PlottableSignalXY : PlottableSignalXYGeneric<double, double>
    {

        public PlottableSignalXY() : base()
        {
            minmaxSearchStrategy = new LinearDoubleOnlyMinMaxStrategy();
        }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.label) ? "" : $" ({this.label})";
            return $"PlottableSignalXY{label} with {GetPointCount()} points";
        }
    }
}
