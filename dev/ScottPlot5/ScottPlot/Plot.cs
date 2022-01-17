using Microsoft.Maui.Graphics;
using System.Collections.Generic;

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
        AddScatter(Generate.Consecutive(51), Generate.Sin(51));
        AddScatter(Generate.Consecutive(51), Generate.Cos(51));
    }

    public Plottable.Scatter<double> AddScatter(double[] xs, double[] ys)
    {
        var sp = new Plottable.Scatter<double>(xs, ys);
        Plottables.Add(sp);
        return sp;
    }

    public void Draw(ICanvas canvas, float width, float height)
    {
        Layout.Resize(width, height);
        RectangleF dataRect = Layout.DataRect;

        AxisLimits2D limits = new(-10, 60, -2, 2);
        PlotView view = new(limits, dataRect);

        if (Layout.HasFigureArea)
            DrawFigure(canvas, view);

        if (Layout.HasDataArea)
            DrawDataArea(canvas, view);
    }

    private void DrawFigure(ICanvas canvas, PlotView view)
    {
        canvas.FillColor = Style.FigureBackgroundColor;
        canvas.FillRectangle(Layout.FigureRect);
    }

    private void DrawDataArea(ICanvas canvas, PlotView view)
    {
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