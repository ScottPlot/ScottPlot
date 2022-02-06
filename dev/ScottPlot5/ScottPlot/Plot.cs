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
    // TODO: move style into figure layout so it can be passed into plottables???
    /// <summary>
    /// Defines styling for the plot such as background color
    /// </summary>
    public readonly PlotStyle Style = new();

    /// <summary>
    /// List of objects drawn on the plot every time a render is requested
    /// </summary>
    public readonly List<Plottable.IPlottable> Plottables = new();

    /// <summary>
    /// Defines the default colors to use when new plottables are added
    /// </summary>
    public Palette Palette = Palettes.Default;

    /// <summary>
    /// Defines plot size, data area, and axis limits.
    /// Stores minimum state necessary.
    /// </summary>
    public PlotInfo LastRenderInfo = PlotInfo.Default;

    public Plot()
    {
    }

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

    #endregion

    #region rendering

    public void Draw(ICanvas canvas)
    {
        Draw(canvas, LastRenderInfo);
    }

    public void Draw(ICanvas canvas, PlotInfo info)
    {
        if (!info.FigureRect.HasPositiveArea)
            return;

        canvas.FillColor = Style.FigureBackgroundColor;
        canvas.FillRectangle(info.FigureRect.RectangleF);

        canvas.FillColor = Style.DataBackgroundColor;
        canvas.FillRectangle(info.DataRect.RectangleF);

        foreach (Plottable.IPlottable plottable in Plottables)
        {
            plottable.Draw(canvas, info);
        }

        canvas.StrokeColor = Style.DataBorderColor;
        canvas.DrawRectangle(info.DataRect.RectangleF);
    }

    public string SaveFig(string path)
    {
        return SaveFig(path, LastRenderInfo);
    }

    public string SaveFig(string path, PlotInfo layout)
    {
        using BitmapExportContext context = SkiaGraphicsService.Instance.CreateBitmapExportContext((int)layout.Width, (int)layout.Height, layout.DisplayScale);
        GraphicsPlatform.RegisterGlobalService(SkiaGraphicsService.Instance);
        Draw(context.Canvas, layout);

        string fullPath = Path.GetFullPath(path);
        using FileStream fs = new(fullPath, FileMode.Create);
        context.WriteToStream(fs);
        return fullPath;
    }

    #endregion
}