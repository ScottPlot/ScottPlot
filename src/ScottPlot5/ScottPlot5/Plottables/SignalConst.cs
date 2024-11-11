using ScottPlot.DataSources;

namespace ScottPlot.Plottables;

public class SignalConst<T> : Signal, IPlottable, IHasLine, IHasMarker, IHasLegendText
    where T : struct, IComparable
{
    public SignalConst(SignalConstSource<T> data) : base(data)
    {
    }

    public SignalConst(T[] ys, double period) : this(new SignalConstSource<T>(ys, period))
    {
    }
}

