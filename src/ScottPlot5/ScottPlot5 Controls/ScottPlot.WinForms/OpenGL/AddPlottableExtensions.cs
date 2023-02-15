namespace ScottPlot;

/// <summary>
/// This class extends Plot.Add.* to add additional plottables provided by this NuGet package
/// </summary>
public static class AddPlottableExtensions
{
    public static void ScatterGL(this AddPlottable add, SkiaSharp.GRContext context, double[] xs, double[] ys)
    {
        DataSources.ScatterSourceXsYs data = new(xs, ys);
        Plottables.ScatterGL sp = new(data, context);
        Color nextColor = add.NextColor;
        sp.LineStyle.Color = nextColor;
        sp.MarkerStyle.Fill.Color = nextColor;
        add.Plottable(sp);
    }
}
