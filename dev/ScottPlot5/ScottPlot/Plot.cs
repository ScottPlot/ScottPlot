using Microsoft.Maui.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace ScottPlot;

public class Plot
{
    public readonly PlotLayout Layout = new();
    public readonly PlotStyle Style = new();
    public readonly List<Plottable.IPlottable> Plottables = new();

    public Plot()
    {
    }

    public void AddDemoSinAndCos()
    {
        double[] xsArray = Generate.Consecutive(51);
        double[] ysArray = Generate.Sin(51);
        AddScatter(xsArray, ysArray, Colors.Yellow);

        List<double> xsList = Generate.Consecutive(51).ToList();
        List<double> ysList = Generate.Cos(51).ToList();
        AddScatterList(xsList, ysList, Colors.Orange);
    }

    public Plottable.ScatterArray<double> AddScatter(double[] xs, double[] ys, Color? color = null)
    {
        Plottable.ScatterArray<double> sp = new(xs, ys)
        {
            LineColor = color ?? Colors.Blue,
            MarkerColor = color ?? Colors.Blue,
        };

        Plottables.Add(sp);
        return sp;
    }

    public Plottable.ScatterList<double> AddScatterList(List<double> xs, List<double> ys, Color? color = null)
    {
        Plottable.ScatterList<double> sp = new(xs, ys)
        {
            LineColor = color ?? Colors.Blue,
            MarkerColor = color ?? Colors.Blue,
        };

        Plottables.Add(sp);
        return sp;
    }

    public void Draw(ICanvas canvas, float width, float height)
    {
        Layout.Resize(width, height);
        RectangleF dataRect = Layout.DataRect;

        AxisLimits2D limits = new(-10, 60, -2, 2);
        PlotView view = new(limits, dataRect);

        if (!Layout.HasFigureArea)
            return;

        canvas.FillColor = Style.FigureBackgroundColor;
        canvas.FillRectangle(Layout.FigureRect);

        if (!Layout.HasDataArea)
            return;

        canvas.FillColor = Style.DataBackgroundColor;
        canvas.FillRectangle(Layout.DataRect);

        foreach (Plottable.IPlottable plottable in Plottables)
        {
            plottable.Draw(canvas, view);
        }

        canvas.StrokeColor = Style.DataBorderColor;
        canvas.DrawRectangle(Layout.DataRect);
    }
}