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
    /// This object holds all the information needed to render a plot at an arbitrary size:
    /// Figure size, data area size, axis limits, tick generator, etc.
    /// </summary>
    public PlotInfo Info { get; set; } = PlotInfo.Default;

    /// <summary>
    /// This object stores information about previous render performance.
    /// </summary>
    public readonly RenderStats Stats = new();

    public Plot()
    {
    }

    #region add/remove plottables

    public void Clear() => Plottables.Clear();

    public Plottables.ScatterArray<double> AddScatter(double[] xs, double[] ys, Color? color = null)
    {
        color ??= Info.Style.Palette.GetColor(Plottables.Count);

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

        if (!totalLimits.HasFiniteWidth)
            totalLimits = totalLimits.WithX(-10, 10);

        if (!totalLimits.HasFiniteHeight)
            totalLimits = totalLimits.WithY(-10, 10);

        Console.WriteLine($"LIMITS: {totalLimits}");
        Info = Info.WithAxisLimits(totalLimits);
    }

    #endregion

    #region Layout and Styling

    private bool TightenLayoutOnNextRender = true;
    public void TightenLayout()
    {
        TightenLayoutOnNextRender = true;
    }

    public void TightenLayout(ICanvas canvas, Tick[]? allTicks = null)
    {
        Axes.IAxis[] GetAxisLabels(Edge edge) => Info.Axes.Where(x => x.Edge == edge).ToArray();

        float padLeft = GetAxisLabels(Edge.Left).Sum(x => x.Label.Measure(canvas).Height);
        float padRight = GetAxisLabels(Edge.Right).Sum(x => x.Label.Measure(canvas).Height);
        float padBottom = GetAxisLabels(Edge.Bottom).Sum(x => x.Label.Measure(canvas).Height);
        float padTop = GetAxisLabels(Edge.Top).Sum(x => x.Label.Measure(canvas).Height);

        if (allTicks is not null)
        {
            float noTickPad = 10; // distance to space text from spine if no ticks exist
            float maxLeftTickWidth = allTicks.Where(x => x.Edge == Edge.Left).Select(x => x.Measure(canvas).Width).DefaultIfEmpty(noTickPad).Max();
            float maxRightTickWidth = allTicks.Where(x => x.Edge == Edge.Right).Select(x => x.Measure(canvas).Width).DefaultIfEmpty(noTickPad).Max();
            float maxBottomTickHeight = allTicks.Where(x => x.Edge == Edge.Bottom).Select(x => x.Measure(canvas).Height).DefaultIfEmpty(noTickPad).Max();
            float maxTopTickHeight = allTicks.Where(x => x.Edge == Edge.Top).Select(x => x.Measure(canvas).Height).DefaultIfEmpty(noTickPad).Max();

            padLeft += maxLeftTickWidth;
            padRight += maxRightTickWidth;
            padBottom += maxBottomTickHeight;
            padTop += maxTopTickHeight;
        }

        Info = Info.WithPadding(padLeft, padRight, padBottom, padTop);
    }

    #endregion

    #region rendering

    public void Draw(ICanvas canvas) => Draw(canvas, Info);

    public void Draw(ICanvas canvas, PlotInfo info)
    {
        if (!info.FigureRect.HasPositiveArea)
            return;

        Stopwatch sw = Stopwatch.StartNew();

        if (TightenLayoutOnNextRender)
        {
            // tighten once without ticks
            TightenLayout(canvas);
        }

        Tick[] bottomTicks = info.TickFactory.GenerateTicks(info, Edge.Bottom);
        Tick[] leftTicks = info.TickFactory.GenerateTicks(info, Edge.Left);
        Tick[] rightTicks = info.TickFactory.GenerateTicks(info, Edge.Right);
        Tick[] topTicks = info.TickFactory.GenerateTicks(info, Edge.Top);

        Tick[] allTicks = bottomTicks.Concat(leftTicks).Concat(rightTicks).Concat(topTicks).ToArray();

        if (TightenLayoutOnNextRender)
        {
            // tighten again now that we have tick sizes
            TightenLayout(canvas, allTicks);
            TightenLayoutOnNextRender = false;
        }

        canvas.FillColor = Info.Style.FigureBackgroundColor;
        canvas.FillRectangle(info.FigureRect.RectangleF);

        if (!info.DataRect.HasPositiveArea)
            return;

        canvas.FillColor = Info.Style.DataBackgroundColor;
        canvas.FillRectangle(info.DataRect.RectangleF);

        foreach (Tick tick in allTicks)
            tick.DrawGridLine(canvas, info);

        foreach (IPlottable plottable in Plottables)
        {
            canvas.SaveState(); // because we can't trust each plottable to do this
            plottable.Draw(canvas, info);
            canvas.RestoreState();
        }

        canvas.StrokeSize = 1;
        canvas.StrokeColor = Info.Style.DataBorderColor;
        canvas.DrawRectangle(info.DataRect.Expand(.5f).RectangleF);

        foreach (Tick tick in allTicks)
            tick.DrawTickAndLabel(canvas, info);

        foreach (Axes.IAxis ax in Info.Axes)
        {
            ax.Draw(canvas, info);
        }

        sw.Stop();
        Stats.AddRenderTime(sw.Elapsed);
        Stats.Draw(canvas, info);
    }

    #endregion

    #region IO

    public string SaveFig(string path)
    {
        return SaveFig(path, Info);
    }

    public string SaveFig(string path, PlotInfo layout)
    {
        using SkiaBitmapExportContext context = new((int)layout.Width, (int)layout.Height, layout.DisplayScale);

        Draw(context.Canvas, layout);

        string fullPath = Path.GetFullPath(path);
        using FileStream fs = new(fullPath, FileMode.Create);
        context.WriteToStream(fs);
        return fullPath;
    }

    #endregion

    #region Testing and Development

    public bool Benchmark(bool enable = true)
    {
        Stats.IsVisible = enable;
        return Stats.IsVisible;
    }

    public bool BenchmarkToggle() => Stats.IsVisible = !Stats.IsVisible;

    #endregion
}