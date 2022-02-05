using Microsoft.Maui.Graphics;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui.Graphics.Skia;
using System.IO;

namespace ScottPlot;

public class Plot
{
    public readonly PlotStyle Style = new();
    public readonly List<Plottable.IPlottable> Plottables = new();
    private PlotView LastView = new();
    public Palette Palette = Palettes.Default;

    public Plot()
    {
        LastView = new(
            limits: new CoordinateRect(-10, 60, -2, 2),
            figure: new RectangleF(0, 0, 400, 300),
            data: new RectangleF(40, 10, 350, 250));
    }

    #region pixel/coordinate relationships

    #endregion

    #region mouse interaction

    public PlotView GetLastView()
    {
        return LastView;
    }

    #endregion

    #region add/remove plottables

    public Plottable.ScatterArray<double> AddScatter(double[] xs, double[] ys, Color? color = null)
    {
        color ??= Palette.GetColor(Plottables.Count);

        Plottable.ScatterArray<double> sp = new(xs, ys)
        {
            LineColor = color,
            MarkerColor = color,
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

    #endregion

    #region rendering

    public void Draw(ICanvas canvas, float width, float height)
    {
        var view = new PlotView()
            .WithSize(width, height)
            .WithPadding(40, 10, 20, 10)
            .WithAxisLimits(-10, 60, -2, 2);

        Debug.WriteLine(view);

        Draw(canvas, view);
    }

    public void Draw(ICanvas canvas, PlotView view)
    {
        if (!view.HasFigureArea)
            return;

        canvas.FillColor = Style.FigureBackgroundColor;
        canvas.FillRectangle(view.FigureRect);

        if (!view.HasDataArea)
            return;

        canvas.FillColor = Style.DataBackgroundColor;
        canvas.FillRectangle(view.DataRect);

        foreach (Plottable.IPlottable plottable in Plottables)
        {
            plottable.Draw(canvas, view, Style);
        }

        canvas.StrokeColor = Style.DataBorderColor;
        canvas.DrawRectangle(view.DataRect);

        LastView = view;
    }

    #endregion

    #region IO

    public string SaveFig(string path, int width, int height, float displayScale = 1.0f)
    {
        using BitmapExportContext context = SkiaGraphicsService.Instance.CreateBitmapExportContext(width, height, displayScale);
        GraphicsPlatform.RegisterGlobalService(SkiaGraphicsService.Instance);
        Draw(context.Canvas, LastView);

        string fullPath = Path.GetFullPath(path);
        using FileStream fs = new(fullPath, FileMode.Create);
        context.WriteToStream(fs);
        return fullPath;
    }

    #endregion
}