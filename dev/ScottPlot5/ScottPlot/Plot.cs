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
    /// <summary>
    /// List of objects drawn on the plot every time a render is requested
    /// </summary>
    public readonly List<IPlottable> Plottables = new();

    /// <summary>
    /// Defines the default colors to use when new plottables are added
    /// </summary>
    public Palette Palette = Palettes.Default;

    /// <summary>
    /// This object holds all the information needed to render a plot at an arbitrary size:
    /// Figure size, data area size, axis limits, tick generator, etc.
    /// </summary>
    public PlotInfo Info { get; set; } = PlotInfo.Default;

    public Plot()
    {
    }

    #region add/remove plottables

    public Plottables.ScatterArray<double> AddScatter(double[] xs, double[] ys, Color? color = null)
    {
        color ??= Palette.GetColor(Plottables.Count);

        Plottables.ScatterArray<double> sp = new(xs, ys)
        {
            LineColor = color,
            MarkerColor = color,
        };

        Plottables.Add(sp);
        return sp;
    }

    #endregion

    #region Axis Manipulation

    public void Autoscale()
    {
        CoordinateRect totalLimits = CoordinateRect.AllNan();
        foreach (IPlottable plottable in Plottables)
        {
            CoordinateRect limits = plottable.GetDataLimits();
            totalLimits = totalLimits.Expand(limits);
        }
        Console.WriteLine($"LIMITS: {totalLimits}");
        Info = Info.WithAxisLimits(totalLimits);
    }

    #endregion

    #region rendering

    public void Draw(ICanvas canvas) => Draw(canvas, Info);

    public void Draw(ICanvas canvas, PlotInfo info)
    {
        if (!info.FigureRect.HasPositiveArea)
            return;

        canvas.FillColor = Info.Style.FigureBackgroundColor;
        canvas.FillRectangle(info.FigureRect.RectangleF);

        if (!info.DataRect.HasPositiveArea)
            return;

        canvas.FillColor = Info.Style.DataBackgroundColor;
        canvas.FillRectangle(info.DataRect.RectangleF);

        foreach (IPlottable plottable in Plottables)
        {
            canvas.SaveState(); // because we can't trust each plottable to do this
            plottable.Draw(canvas, info);
            canvas.RestoreState();
        }

        canvas.StrokeColor = Info.Style.DataBorderColor;
        canvas.DrawRectangle(info.DataRect.RectangleF);


        foreach (Tick tick in info.TickFactory.GenerateTicks(info, Edge.Bottom))
            tick.Draw(canvas, info);

        foreach (Tick tick in info.TickFactory.GenerateTicks(info, Edge.Left))
            tick.Draw(canvas, info);
    }

    #endregion

    #region IO

    public string SaveFig(string path)
    {
        return SaveFig(path, Info);
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