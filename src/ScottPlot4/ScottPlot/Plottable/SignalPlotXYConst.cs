using ScottPlot.MinMaxSearchStrategies;
using System;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// A variation of the SignalPlotConst optimized for unevenly-spaced ascending X values.
    /// </summary>
    /// <typeparam name="TX"></typeparam>
    /// <typeparam name="TY"></typeparam>
    public class SignalPlotXYConst<TX, TY> : SignalPlotXYGeneric<TX, TY> where TX : struct, IComparable where TY : struct, IComparable
    {
        public bool TreesReady => (Strategy as SegmentedTreeMinMaxSearchStrategy<TY>)?.TreesReady ?? false;

        public SignalPlotXYConst() : base()
        {
            Strategy = new SegmentedTreeMinMaxSearchStrategy<TY>();
        }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.Label) ? "" : $" ({this.Label})";
            return $"PlottableSignalXYConst{label} with {PointCount} points ({typeof(TX).Name}, {typeof(TY).Name})";
        }
    }
}
