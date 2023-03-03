using ScottPlot.Control;
using ScottPlot.WinForms.OpenGL;
using System;

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


/* Необъединенное слияние из проекта "ScottPlot.WinForms (net6.0-windows)"
До:
    public static Plottables.ScatterGLCustomWidth ScatterGLCustomWidth(this AddPlottable add, IPlotControl control, double[] xs, double[] ys)
После:
    public static WinForms.OpenGL.ScatterGLCustomWidth ScatterGLCustomWidth(this AddPlottable add, IPlotControl control, double[] xs, double[] ys)
*/
    public static ScatterGLCustomWidth ScatterGLCustomWidth(this AddPlottable add, IPlotControl control, double[] xs, double[] ys)
    {
        DataSources.ScatterSourceXsYs data = new(xs, ys);

/* Необъединенное слияние из проекта "ScottPlot.WinForms (net6.0-windows)"
До:
        Plottables.ScatterGLCustomWidth sp = new(data, control);
После:
        WinForms.OpenGL.ScatterGLCustomWidth sp = new(data, control);
*/
        ScatterGLCustomWidth sp = new(data, control);
        Color nextColor = add.NextColor;
        sp.LineStyle.Color = nextColor;
        sp.MarkerStyle.Fill.Color = nextColor;
        add.Plottable(sp);
        return sp;
    }
}
