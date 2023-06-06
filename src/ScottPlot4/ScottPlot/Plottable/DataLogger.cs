using ScottPlot.Plottable.DataViewManagers;
using System.Collections.Generic;
using System;
using System.Drawing;
using System.Linq;

namespace ScottPlot.Plottable;

public class DataLogger : IPlottable
{
    public bool IsVisible { get; set; } = true;
    public int XAxisIndex { get; set; } = 0;
    public int YAxisIndex { get; set; } = 0;
    public string Label { get; set; } = string.Empty;
    public Color Color { get; set; } = Color.Blue;
    public float LineWidth { get; set; } = 1;
    public bool ManageAxisLimits { get; set; } = true;
    private IDataViewManager ViewManager { get; set; } = new Full();

    private readonly List<Coordinate> Data = new();
    public int Count => Data.Count;
    public int CountOnLastRender { get; private set; } = -1;
    public double DataMinX { get; private set; } = double.PositiveInfinity;
    public double DataMaxX { get; private set; } = double.NegativeInfinity;
    public double DataMinY { get; private set; } = double.PositiveInfinity;
    public double DataMaxY { get; private set; } = double.NegativeInfinity;

    public Plot Plot { get; private set; }

    public DataLogger(Plot plot)
    {
        Plot = plot;
    }

    public void Add(Coordinate coordinate)
    {
        Add(coordinate.X, coordinate.Y);
    }

    public void Add(double x, double y)
    {
        Coordinate coord = new(x, y);
        Data.Add(coord);
        DataMinX = Math.Min(coord.X, DataMinX);
        DataMaxX = Math.Max(coord.X, DataMaxX);
        DataMinY = Math.Min(coord.Y, DataMinY);
        DataMaxY = Math.Max(coord.Y, DataMaxY);
    }

    public void AddRange(IEnumerable<Coordinate> coordinates)
    {
        foreach (Coordinate coordinate in coordinates)
        {
            Add(coordinate);
        }
    }

    public void Clear()
    {
        Data.Clear();
        DataMinX = double.PositiveInfinity;
        DataMaxX = double.NegativeInfinity;
        DataMinY = double.PositiveInfinity;
        DataMaxY = double.NegativeInfinity;
    }

    public AxisLimits GetAxisLimits()
    {
        return Data.Any()
            ? new AxisLimits(DataMinX, DataMaxX, DataMinY, DataMaxY)
            : AxisLimits.NoLimits;
    }

    public LegendItem[] GetLegendItems() => LegendItem.Single(this, Label, Color);

    public void ValidateData(bool deep = false) { }

    public void ViewFull()
    {
        ViewManager = new Full();
        UpdateAxisLimits(true);
    }

    public void ViewJump(double paddingFraction = .5)
    {
        ViewManager = new Slide()
        {
            PaddingFractionX = paddingFraction
        };
        UpdateAxisLimits(true);
    }

    public void ViewSlide()
    {
        ViewManager = new Slide()
        {
            PaddingFractionX = 0
        };
        UpdateAxisLimits(true);
    }

    public void ViewCustom(IDataViewManager viewManager)
    {
        ViewManager = viewManager;
        UpdateAxisLimits(true);
    }

    private void UpdateAxisLimits(bool force = false)
    {
        AxisLimits viewLimits = force ? AxisLimits.NoLimits : Plot.GetAxisLimits(XAxisIndex, YAxisIndex);
        AxisLimits dataLimits = GetAxisLimits();
        AxisLimits newLimits = ViewManager.GetAxisLimits(viewLimits, dataLimits);
        Plot.SetAxisLimits(newLimits);

        if (force)
            UpdateAxisLimits();
    }

    public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
    {
        CountOnLastRender = Count;

        if (ManageAxisLimits)
            UpdateAxisLimits();

        using var gfx = ScottPlot.Drawing.GDI.Graphics(bmp, dims, lowQuality);
        using var pen = ScottPlot.Drawing.GDI.Pen(Color, LineWidth, LineStyle.Solid);

        PointF[] points = Data.Select(x => dims.GetPixel(x).ToPointF()).ToArray();
        if (points.Length > 0)
        {
            gfx.DrawLines(pen, points);
        }
    }
}
