using System.Collections.Generic;
using System.Drawing;
using System.Linq;

#nullable enable

namespace ScottPlot.Plottable;

/// <summary>
/// Data logging scatter plot.
/// This plot type stores 2D coordinates and has methods to add and remove points.
/// </summary>
public class ScatterDataLogger : IPlottable, IDataLogger
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
    public IDataLoggerView LoggerView { get; set; } = new DataLoggerViews.FullLoggerView();
    public Plot Plot { get; private set; }

    public ScatterDataLogger(Plot plot) { Plot = plot; }

    private readonly List<Coordinate> DataPoints = new();

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
        if (ManageAxisLimits)
            LoggerView.SetAxisLimits(Plot, DataLimits);

        LastRenderCount = Count;

        PointF[] points = DataPoints
            .Select(x => x.ToPixel(dims))
            .Select(px => new PointF(px.X, px.Y))
            .ToArray();

        using var gfx = ScottPlot.Drawing.GDI.Graphics(bmp, dims, lowQuality);
        using var pen = ScottPlot.Drawing.GDI.Pen(Color, LineWidth, LineStyle.Solid);
        gfx.DrawLines(pen, points);
    }
}
