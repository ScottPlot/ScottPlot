using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ScottPlot.Plottable.DataLoggerViews;

#nullable enable

namespace ScottPlot.Plottable;

/// <summary>
/// Data logging scatter plot designed for growing datasets.
/// </summary>
public class DataLogger : IPlottable, IDataLogger
{
    public bool IsVisible { get; set; } = true;
    public int XAxisIndex { get; set; } = 0;
    public int YAxisIndex { get; set; } = 0;
    public int Count => DataPoints.Count;
    public int LastRenderCount { get; private set; } = -1;
    public AxisLimits DataLimits { get; private set; } = AxisLimits.NoLimits;
    public string Label { get; set; } = string.Empty;
    public Color Color { get; set; } = Color.Blue;
    public float LineWidth { get; set; } = 1;
    public bool ManageAxisLimits { get; set; } = true;

    private IDataLoggerView _LoggerView = new DataLoggerViews.Full();
    public IDataLoggerView LoggerView
    {
        get => _LoggerView;
        set
        {
            _LoggerView = value;
            Plot.SetAxisLimits(DataLimits);
        }
    }

    public Plot Plot { get; private set; }

    public DataLogger(Plot plot)
    {
        Plot = plot;
    }

    private readonly List<Coordinate> DataPoints = new();

    /// <summary>
    /// If set, ignore the X values
    /// </summary>
    public double? SamplePeriod { get; set; } = null;

    /// <summary>
    /// If signal mode is enabled and SamplePeriod is set, display this many of the latest points
    /// </summary>
    public int SignalPointCount { get; set; } = 1000;

    /// <summary>
    /// If enabled, use a "wipe" display and only show data from the latest <see cref="FixedWidth"/>
    /// </summary>
    public double? FixedWidth { get; set; } = null;

    public void Clear()
    {
        DataPoints.Clear();
        DataLimits = AxisLimits.NoLimits;
    }

    public void Add(Coordinate coordinate)
    {
        DataPoints.Add(coordinate);
        DataLimits = DataLimits.Expand(coordinate);
    }

    public void Add(double x, double y) => Add(new Coordinate(x, y));

    public AxisLimits GetAxisLimits() => DataLimits;

    public LegendItem[] GetLegendItems() => LegendItem.Single(this, Label, Color);

    public void ValidateData(bool deep = false) { }

    public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
    {
        LastRenderCount = Count;

        if (SamplePeriod.HasValue)
        {
            RenderSignal(dims, bmp, lowQuality);
        }
        else
        {
            RenderXY(dims, bmp, lowQuality);
        }
    }

    private void RenderSignal(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
    {
        if (DataPoints.Count < SignalPointCount)
            return; // TODO: support partial rendering

        double viewWidth = SignalPointCount * SamplePeriod!.Value * 1;
        double[] ysInView = DataPoints.Skip(DataPoints.Count - SignalPointCount).Select(x => x.Y).ToArray();

        if (ysInView.Length != SignalPointCount)
            throw new InvalidOperationException();

        if (ManageAxisLimits)
            Plot.SetAxisLimits(0, viewWidth, ysInView.Min(), ysInView.Max());

        int divIndex = DataPoints.Count % SignalPointCount;
        double divOffset = divIndex * SamplePeriod!.Value;
        int oldCount = ysInView.Length - divIndex;
        int newCount = ysInView.Length - oldCount;

        PointF[] oldest = Enumerable
            .Range(0, oldCount)
            .Select(x => new Coordinate(x * SamplePeriod!.Value + divOffset, ysInView[x]))
            .Select(x => x.ToPixel(dims))
            .Select(x => x.ToPointF())
            .ToArray();

        PointF[] newest = Enumerable
            .Range(0, newCount)
            .Select(x => new Coordinate(x * SamplePeriod!.Value, ysInView[ysInView.Length - divIndex + x - 1]))
            .Select(x => x.ToPixel(dims))
            .Select(x => x.ToPointF())
            .ToArray();

        using var gfx = ScottPlot.Drawing.GDI.Graphics(bmp, dims, lowQuality);
        using var pen = ScottPlot.Drawing.GDI.Pen(Color, LineWidth, LineStyle.Solid);

        if (oldest.Any())
            gfx.DrawLines(pen, oldest);

        if (newest.Any())
            gfx.DrawLines(pen, newest);
    }

    public void RenderXY(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
    {
        if (FixedWidth is null)
        {
            if (ManageAxisLimits)
                LoggerView.SetAxisLimits(Plot, DataLimits);

            RenderXY_All(dims, bmp, lowQuality);
        }
        else
        {
            double viewMinX = DataPoints.Last().X - FixedWidth.Value;
            int firstViewIndex = 0;
            while (DataPoints[firstViewIndex].X < viewMinX)
                firstViewIndex += 1;

            List<Coordinate> pointsInView = DataPoints
                .Skip(firstViewIndex)
                .Select(pt => new Coordinate(pt.X - viewMinX, pt.Y))
                .ToList();

            if (ManageAxisLimits)
            {
                AxisLimits pointsInViewLimits = pointsInView.GetLimits();
                Plot.SetAxisLimits(0, FixedWidth, pointsInViewLimits.YMin, pointsInViewLimits.YMax);
            }

            RenderXY_Latest(dims, bmp, lowQuality, pointsInView);
        }
    }

    public void RenderXY_All(PlotDimensions dims, Bitmap bmp, bool lowQuality)
    {
        PointF[] points = DataPoints
            .Select(x => x.ToPixel(dims))
            .Select(px => new PointF(px.X, px.Y))
            .ToArray();

        using var gfx = ScottPlot.Drawing.GDI.Graphics(bmp, dims, lowQuality);
        using var pen = ScottPlot.Drawing.GDI.Pen(Color, LineWidth, LineStyle.Solid);
        gfx.DrawLines(pen, points);
    }

    public void RenderXY_Latest(PlotDimensions dims, Bitmap bmp, bool lowQuality, List<Coordinate> pointsInView)
    {
        PointF[] points = pointsInView
            .Select(x => x.ToPixel(dims))
            .Select(px => new PointF(px.X, px.Y))
            .ToArray();

        using var gfx = ScottPlot.Drawing.GDI.Graphics(bmp, dims, lowQuality);
        using var pen = ScottPlot.Drawing.GDI.Pen(Color, LineWidth, LineStyle.Solid);
        gfx.DrawLines(pen, points);
    }
}
