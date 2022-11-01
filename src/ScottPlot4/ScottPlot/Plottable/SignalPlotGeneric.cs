using ScottPlot.MinMaxSearchStrategies;
using System;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// A signal plot displays evenly-spaced data.
    /// Instead of X/Y pairs, signal plots take Y values and a sample rate.
    /// Optional X and Y offsets can further customize the data.
    /// </summary>
    public class SignalPlotGeneric<T> : SignalPlotBase<T> where T : struct, IComparable
    {
        public SignalPlotGeneric() : base()
        {
            Strategy = new MinMaxSearchStrategies.LinearMinMaxSearchStrategy<T>();
        }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.Label) ? "" : $" ({this.Label})";
            return $"PlottableSignal{label} with {PointCount} points";
        }
    }
}
