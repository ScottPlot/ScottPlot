using ScottPlot.MinMaxSearchStrategies;
using System;
using System.Drawing;

namespace ScottPlot
{
    public class PlottableSignalXYConst<TX, TY> : PlottableSignalXYGeneric<TX, TY> where TX : struct, IComparable where TY : struct, IComparable
    {
        public bool TreesReady => (minmaxSearchStrategy as SegmentedTreeMinMaxSearchStrategy<TY>)?.TreesReady ?? false;
        public PlottableSignalXYConst(TX[] xs, TY[] ys, Color color, double lineWidth, double markerSize, string label, int minRenderIndex, int maxRenderIndex, LineStyle lineStyle, bool useParallel, IMinMaxSearchStrategy<TY> minMaxSearchStrategy = null)
            : base(xs, ys, color, lineWidth, markerSize, label, minRenderIndex, maxRenderIndex, lineStyle, useParallel, new SegmentedTreeMinMaxSearchStrategy<TY>())
        {

        }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.label) ? "" : $" ({this.label})";
            return $"PlottableSignalXYConst{label} with {GetPointCount()} points ({typeof(TX).Name}, {typeof(TY).Name})";
        }
    }
}
