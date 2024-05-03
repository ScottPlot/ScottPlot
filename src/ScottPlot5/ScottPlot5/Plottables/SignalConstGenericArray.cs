using ScottPlot.DataSources;

namespace ScottPlot.Plottables;

public class SignalConstGenericArray<T>(T[] ys, double period) : SignalConstBase, IPlottable
    where T : struct, IComparable
{
    readonly SignalConstSourceGenericArray<T> Data = new(ys, period);

    public AxisLimits GetAxisLimits() => Data.GetAxisLimits();

    public virtual void Render(RenderPack rp)
    {
        List<PixelColumn> cols = Data.GetPixelColumns(Axes);
        Render(rp, cols);
    }
}
