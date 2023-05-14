using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ScottPlot.Plottable;

public class ScatterLogger : IPlottable
{
    public bool IsVisible { get; set; } = true;
    public int XAxisIndex { get; set; } = 0;
    public int YAxisIndex { get; set; } = 0;
    public int Count => DataPoints.Count();
    public int LastRenderCount { get; private set; } = -1;
    public string Label { get; set; } = string.Empty;
    public Color Color { get; set; } = Color.Blue;
    public float LineWidth { get; set; } = 1;

    // data management
    private readonly List<Coordinate> DataPoints = new();

    public void Clear() => DataPoints.Clear();

    public void Add(Coordinate coordinate) => DataPoints.Add(coordinate);

    public void AddRange(IEnumerable<Coordinate> coordinates) => DataPoints.AddRange(coordinates);

    public void Add(double x, double y) => DataPoints.Add(new Coordinate(x, y));

    public void Add(DateTime x, double y) => DataPoints.Add(new Coordinate(x.ToOADate(), y));

    public void AddRange(double[] xs, double[] ys)
    {
        if (xs.Length != ys.Length) throw new ArgumentException("Xs and Ys must have same length");
        var coordinates = Enumerable.Range(0, xs.Length).Select(x => new Coordinate(xs[x], ys[x]));
        AddRange(coordinates);
    }

    public void AddRange(DateTime[] xs, double[] ys)
    {
        if (xs.Length != ys.Length) throw new ArgumentException("Xs and Ys must have same length");
        var coordinates = Enumerable.Range(0, xs.Length).Select(x => new Coordinate(xs[x].ToOADate(), ys[x]));
        AddRange(coordinates);
    }

    // axis limit management

    public void UpdateAxisLimits(Plot plt)
    {
        AxisLimits currentLimits = plt.GetAxisLimits();
        AxisLimits dataLimits = GetAxisLimits();
        UpdateAxisLimitsX(plt, currentLimits, dataLimits);
        UpdateAxisLimitsY(plt, currentLimits, dataLimits);
    }

    /// <summary>
    /// If data extends off the page to the right, extend the view to the right only
    /// </summary>
    private void UpdateAxisLimitsX(Plot plt, AxisLimits currentLimits, AxisLimits dataLimits, double expandFrac = 1.25)
    {
        if (currentLimits.XMax < dataLimits.XMax)
        {
            plt.SetAxisLimitsX(dataLimits.XMin, dataLimits.XMax * expandFrac);
        }
    }

    /// <summary>
    /// If the data extends off the page vertically, zoom out vertically
    /// </summary>
    private void UpdateAxisLimitsY(Plot plt, AxisLimits currentLimits, AxisLimits dataLimits, double expandFrac = 1.25)
    {
        if (currentLimits.YMin > dataLimits.YMin || currentLimits.YMax < dataLimits.YMax)
        {
            double ySpanHalf = (dataLimits.YSpan / 2) * expandFrac;
            double yMin = dataLimits.YCenter - ySpanHalf;
            double yMax = dataLimits.YCenter + ySpanHalf;
            plt.SetAxisLimitsY(yMin, yMax);
        }
    }

    // plottable methods
    public AxisLimits GetAxisLimits() => DataPoints.GetLimits();
    public LegendItem[] GetLegendItems() => LegendItem.Single(this, Label, Color);
    public void ValidateData(bool deep = false) { }
    public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
    {
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
