using ScottPlot.DataSources;

namespace ScottPlot.Plottables;

[Obsolete("SignalConst has been deprecated, " +
    "but its functionality may be achieved by creating a Signal plot with a SignalConstSource data source. " +
    "See the Add.SignalConst() method for reference.", true)]
public class SignalConst<T>(SignalConstSource<T> data) : Signal(data), IPlottable, IHasLine, IHasMarker, IHasLegendText
    where T : struct, IComparable
{
}

