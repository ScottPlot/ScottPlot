using ScottPlot.Plottable.AxisManagers;
using System.Collections.Generic;
using System;
using System.Drawing;
using System.Linq;

namespace ScottPlot.Plottable;

/// <summary>
/// This plot type stores infinitely growing X/Y pairs and displays them as a scatter plot.
/// </summary>
public class DataLogger : IPlottable, IHasColor, IHasLine, IHasMarker
{
    public bool IsVisible { get; set; } = true;
    public int XAxisIndex { get; set; } = 0;
    public int YAxisIndex { get; set; } = 0;
    public string Label { get; set; } = string.Empty;
    public Color Color { get => LineColor; set { LineColor = value; MarkerColor = value; } }
    public Color LineColor { get; set; } = Color.Blue;
    public Color MarkerColor { get; set; } = Color.Blue;
    public double LineWidth { get; set; } = 1;
    public LineStyle LineStyle { get; set; } = LineStyle.Solid;
    public MarkerShape MarkerShape { get; set; } = MarkerShape.filledCircle;
    public float MarkerSize { get; set; } = 0;
    public float MarkerLineWidth { get; set; } = 1;

    /// <summary>
    /// Number of data points currently being tracked.
    /// </summary>
    public int Count => Data.Count;

    /// <summary>
    /// Number of data points displayed the last time this plottable was rendered.
    /// This can be compared with <see cref="Count"/> to determine if a new render is required.
    /// </summary>
    public int CountOnLastRender { get; private set; } = -1;

    /// <summary>
    /// If true, the <see cref="AxisManager"/> will be used to update axis limits on every render.
    /// </summary>
    public bool ManageAxisLimits { get; set; } = true;

    /// <summary>
    /// Holds logic used to update axis limits on every render if <see cref="ManageAxisLimits"/> is true.
    /// </summary>
    private IAxisManager AxisManager { get; set; } = new Full();

    /// <summary>
    /// Used to obtain the current axis limits so <see cref="AxisManager"/> can adjust them if needed.
    /// </summary>
    public Plot Plot { get; private set; }

    private readonly List<Coordinate> Data = new();
    private double DataMinX = double.PositiveInfinity;
    private double DataMaxX = double.NegativeInfinity;
    private double DataMinY = double.PositiveInfinity;
    private double DataMaxY = double.NegativeInfinity;

    public DataLogger(Plot plot)
    {
        Plot = plot;
    }

    /// <summary>
    /// Add a point to the logger
    /// </summary>
    public void Add(Coordinate coordinate)
    {
        Add(coordinate.X, coordinate.Y);
    }

    /// <summary>
    /// Add a point to the logger
    /// </summary>
    public void Add(double x, double y)
    {
        Coordinate coord = new(x, y);
        Data.Add(coord);
        DataMinX = Math.Min(coord.X, DataMinX);
        DataMaxX = Math.Max(coord.X, DataMaxX);
        DataMinY = Math.Min(coord.Y, DataMinY);
        DataMaxY = Math.Max(coord.Y, DataMaxY);
    }

    /// <summary>
    /// Add a collection of points to the logger
    /// </summary>
    public void AddRange(IEnumerable<Coordinate> coordinates)
    {
        foreach (Coordinate coordinate in coordinates)
        {
            Add(coordinate);
        }
    }

    /// <summary>
    /// Clear all logged data points
    /// </summary>
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

    public LegendItem[] GetLegendItems()
    {
        var singleItem = new LegendItem(this)
        {
            label = Label,
            color = LineColor,
            lineStyle = LineStyle,
            lineWidth = LineWidth,
            markerShape = MarkerShape,
            markerSize = MarkerSize,
        };
        return LegendItem.Single(singleItem);
    }

    public void ValidateData(bool deep = false) { }

    /// <summary>
    /// Automatically expand the axis as needed to ensure the full dataset is visible before each render.
    /// </summary>
    public void ViewFull()
    {
        ManageAxisLimits = true;
        AxisManager = new Full();
        UpdateAxisLimits(true);
    }

    /// <summary>
    /// Automatically adjust the axis limits to track the newest data as it comes in.
    /// The axis limits will appear to "jump" when new data runs off the screen.
    /// </summary>
    public void ViewJump(double width = 1000, double paddingFraction = .5)
    {
        ManageAxisLimits = true;
        AxisManager = new Slide()
        {
            Width = width,
            PaddingFractionX = paddingFraction,
        };
        UpdateAxisLimits(true);
    }

    /// <summary>
    /// Automatically adjust the axis limits to track the newest data as it comes in.
    /// The axis limits will appear to "slide" continuously as new data is added.
    /// </summary>
    public void ViewSlide(double width = 1000)
    {
        ManageAxisLimits = true;
        AxisManager = new Slide()
        {
            Width = width,
            PaddingFractionX = 0,
        };
        UpdateAxisLimits(true);
    }

    /// <summary>
    /// Use a custom axis manager to update axis limits before each render.
    /// </summary>
    public void ViewCustom(IAxisManager axisManager)
    {
        ManageAxisLimits = true;
        AxisManager = axisManager;
        UpdateAxisLimits(true);
    }

    private void UpdateAxisLimits(bool force = false)
    {
        AxisLimits viewLimits = force ? AxisLimits.NoLimits : Plot.GetAxisLimits(XAxisIndex, YAxisIndex);
        AxisLimits dataLimits = GetAxisLimits();
        AxisLimits newLimits = AxisManager.GetAxisLimits(viewLimits, dataLimits);
        Plot.SetAxisLimits(newLimits, XAxisIndex, YAxisIndex);

        if (force)
            UpdateAxisLimits();
    }

    public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
    {
        CountOnLastRender = Count;

        if (ManageAxisLimits)
            UpdateAxisLimits();

        using var gfx = ScottPlot.Drawing.GDI.Graphics(bmp, dims, lowQuality);
        using var pen = ScottPlot.Drawing.GDI.Pen(Color, LineWidth, LineStyle);

        PointF[] points = Data.Select(x => dims.GetPixel(x).ToPointF()).ToArray();
        if (points.Length > 1)
        {
            gfx.DrawLines(pen, points);
            // draw a marker at each point
            if ((MarkerSize > 0) && (MarkerShape != MarkerShape.none))
            {
                MarkerTools.DrawMarkers(gfx, points, MarkerShape, MarkerSize, MarkerColor, MarkerLineWidth);
            }
        }
    }
}
