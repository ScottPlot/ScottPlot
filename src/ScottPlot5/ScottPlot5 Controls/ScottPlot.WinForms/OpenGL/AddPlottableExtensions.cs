using ScottPlot.Control;
using ScottPlot.DataSources;

namespace ScottPlot;

/// <summary>
/// This class extends Plot.Add.* to add additional plottables provided by this NuGet package
/// </summary>
public static class AddPlottableExtensions
{
    public static Plottables.ScatterGL ScatterGL(this AddPlottable add, IPlotControl control, double[] xs, double[] ys)
    {
        ScatterSourceXsYs data = new(xs, ys);
        var cachedData = new CacheScatterLimitsDecorator(data);
        Plottables.ScatterGL sp = new(cachedData, control);
        Color nextColor = add.NextColor;
        sp.LineStyle.Color = nextColor;
        sp.MarkerStyle.Fill.Color = nextColor;
        add.Plottable(sp);
        return sp;
    }
}
