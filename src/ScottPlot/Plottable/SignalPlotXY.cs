using ScottPlot.MinMaxSearchStrategies;

namespace ScottPlot.Plottable
{
    public class SignalPlotXY : SignalPlotXYGeneric<double, double>
    {

        public SignalPlotXY() : base()
        {
            minmaxSearchStrategy = new LinearDoubleOnlyMinMaxStrategy();
        }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.label) ? "" : $" ({this.label})";
            return $"PlottableSignalXY{label} with {PointCount} points";
        }
    }
}
