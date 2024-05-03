using ScottPlot.DataSources;

namespace ScottPlot.Plottables;

public class SignalConstDoubleArray(double[] ys, double period) : SignalConstBase, IPlottable
{
    readonly SignalConstSourceDoubleArray Data = new(ys, period);

    public AxisLimits GetAxisLimits() => Data.GetAxisLimits();

    public virtual void Render(RenderPack rp)
    {
        List<PixelColumn> cols = Data.GetPixelColumns(Axes);
        Render(rp, cols);
    }
}
