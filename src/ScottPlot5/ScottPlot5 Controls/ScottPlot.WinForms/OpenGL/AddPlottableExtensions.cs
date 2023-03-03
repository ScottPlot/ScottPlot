using ScottPlot.Control;

namespace ScottPlot;

/// <summary>
/// This class extends Plot.Add.* to add additional plottables provided by this NuGet package
/// </summary>
public static class AddPlottableExtensions
{
    public static Plottables.ScatterGL ScatterGL(this AddPlottable add, IPlotControl control, double[] xs, double[] ys)
    {
        DataSources.ScatterSourceXsYs data = new(xs, ys);
        Plottables.ScatterGL sp = new(data, control);
        Color nextColor = add.NextColor;
        sp.LineStyle.Color = nextColor;
        sp.MarkerStyle.Fill.Color = nextColor;
        add.Plottable(sp);
        return sp;
    }

    public static Plottables.ScatterGLCustomWidth ScatterGLCustomWidth(this AddPlottable add, IPlotControl control, double[] xs, double[] ys)
    {
        DataSources.ScatterSourceXsYs data = new(xs, ys);

        Plottables.ScatterGLCustomWidth sp = new(data, control);
        Color nextColor = add.NextColor;
        sp.LineStyle.Color = nextColor;
        sp.MarkerStyle.Fill.Color = nextColor;
        add.Plottable(sp);
        return sp;
    }
}
