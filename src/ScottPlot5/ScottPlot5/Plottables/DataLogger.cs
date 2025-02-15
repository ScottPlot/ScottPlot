using ScottPlot.AxisLimitManagers;
using ScottPlot.DataSources;

namespace ScottPlot.Plottables;

public class DataLogger(List<Coordinates>? coordinates = null) : IPlottable, IManagesAxisLimits, IHasLine, IHasMarker, IHasLegendText
{
    public DataLoggerSource Data { get; } = new DataLoggerSource(coordinates ?? []);

    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public IAxisLimitManager AxisManager { get; set; } = new Full();
    public bool Rotated { get; set; }
    public LineStyle LineStyle { get; set; } = new() { Width = 1 };
    public float LineWidth { get => LineStyle.Width; set => LineStyle.Width = value; }
    public LinePattern LinePattern { get => LineStyle.Pattern; set => LineStyle.Pattern = value; }
    public Color LineColor { get => LineStyle.Color; set => LineStyle.Color = value; }

    public MarkerStyle MarkerStyle { get; set; } = new() { Size = 0, Shape = MarkerShape.FilledCircle };
    public MarkerShape MarkerShape { get => MarkerStyle.Shape; set => MarkerStyle.Shape = value; }
    public float MarkerSize { get => MarkerStyle.Size; set => MarkerStyle.Size = value; }
    public Color MarkerFillColor { get => MarkerStyle.FillColor; set => MarkerStyle.FillColor = value; }
    public Color MarkerLineColor { get => MarkerStyle.LineColor; set => MarkerStyle.LineColor = value; }
    public Color MarkerColor { get => MarkerStyle.MarkerColor; set => MarkerStyle.MarkerColor = value; }
    public float MarkerLineWidth { get => MarkerStyle.LineWidth; set => MarkerStyle.LineWidth = value; }

    public bool HasNewData => Data.HasNewData;

    public bool InvertX { get; set; } = false;
    public bool InvertY { get; set; } = false;

    /// <summary>
    /// The style of lines to use when connecting points.
    /// </summary>
    public ConnectStyle ConnectStyle { get; set; } = ConnectStyle.Straight;

    public Color Color
    {
        get => LineStyle.Color;
        set
        {
            LineStyle.Color = value;
            MarkerStyle.FillColor = value;
            MarkerStyle.LineColor = value;
        }
    }

    public double Period { get; set; } = 1.0;

    [Obsolete("use LegendText")]
    public string Label { get => LegendText; set => LegendText = value; }
    public string LegendText { get; set; } = string.Empty;

    public IEnumerable<LegendItem> LegendItems => LegendItem.Single(this, LegendText, LineStyle, MarkerStyle);

    public AxisLimits GetAxisLimits()
    {
        CoordinateRange rangeX = Data.GetRangeX();
        CoordinateRange rangeY = Data.GetRangeY(rangeX);

        return Rotated
            ? new AxisLimits(rangeY, rangeX)
            : new AxisLimits(rangeX, rangeY);
    }


    /// <summary>
    /// Get the data point nearest to the given mouse location.
    /// </summary>
    /// <param name="mouseLocation">Mouse location</param>
    /// <param name="dataRect">Data rectangle from RenderDetails</param>
    /// <param name="maxDistance">Maximum distance to the point</param>
    /// <returns></returns>
    public DataPoint GetNearest(Coordinates mouseLocation, PixelRect dataRect, float maxDistance = 15)
    {
        double pxPerUnitX = dataRect.Width / Axes.XAxis.GetRange().Span;
        double pxPerUnitY = dataRect.Height / Axes.YAxis.GetRange().Span;

        double maxDistanceSquared = maxDistance * maxDistance;
        double closestDistanceSquared = double.PositiveInfinity;

        int closestIndex = 0;
        double closestX = double.PositiveInfinity;
        double closestY = double.PositiveInfinity;

        for (int i = 0; i < Data.Coordinates.Count; i++)
        {
            double x = Data.Coordinates[i].X + Data.XOffset;
            double y = Data.Coordinates[i].Y * Data.YScale + Data.YOffset;

            if (Rotated)
                (x, y) = (y, x);

            double dX = (x - mouseLocation.X) * pxPerUnitX;
            double dY = (y - mouseLocation.Y) * pxPerUnitY;
            double distanceSquared = dX * dX + dY * dY;

            if (distanceSquared <= closestDistanceSquared)
            {
                closestDistanceSquared = distanceSquared;
                closestX = x;
                closestY = y;
                closestIndex = i;
            }
        }

        return closestDistanceSquared <= maxDistanceSquared
            ? Rotated ? new DataPoint(closestY, closestX, closestIndex) : new DataPoint(closestX, closestY, closestIndex)
            : DataPoint.None;
    }

    /// <summary>
    /// Get the data point nearest to the given mouse location (only considering X values).
    /// </summary>
    /// <param name="mouseLocation">Mouse location</param>
    /// <param name="dataRect">Data rectangle from RenderDetails</param>
    /// <param name="maxDistance">Maximum X distance to the point</param>
    /// <returns></returns>
    public DataPoint GetNearestX(Coordinates mouseLocation, PixelRect dataRect, float maxDistance = 15)
    {
        double pxPerUnit;

        if (Rotated)
        {
            mouseLocation = mouseLocation.Rotated;
            pxPerUnit = dataRect.Height / Axes.YAxis.GetRange().Span;
        }
        else
        {
            pxPerUnit = dataRect.Width / Axes.XAxis.GetRange().Span;
        }

        int i = Data.GetIndex(mouseLocation.X); // TODO: check the index after too?
        double distance = (Data.Coordinates[i].X + Data.XOffset - mouseLocation.X) * pxPerUnit;
        return distance <= maxDistance
            ? new DataPoint(Data.Coordinates[i].X, Data.Coordinates[i].Y, i)
            : DataPoint.None;
    }

    public virtual void Render(RenderPack rp)
    {
        Pixel[] markerPixels = Rotated ? Data.GetPixelsToDrawVertically(rp, Axes, ConnectStyle)
                                       : Data.GetPixelsToDrawHorizontally(rp, Axes, ConnectStyle);

        Pixel[] linePixels = ConnectStyle switch
        {
            ConnectStyle.Straight => markerPixels,
            ConnectStyle.StepHorizontal => Scatter.GetStepDisplayPixels(markerPixels, true),
            ConnectStyle.StepVertical => Scatter.GetStepDisplayPixels(markerPixels, false),
            _ => throw new NotSupportedException($"unsupported {nameof(ConnectStyle)}: {ConnectStyle}"),
        };

        using SKPaint paint = new();
        Drawing.DrawLines(rp.Canvas, paint, linePixels, LineStyle);
        Drawing.DrawMarkers(rp.Canvas, paint, markerPixels, MarkerStyle);

        Data.OnRendered();
    }

    public void Add(Coordinates coordinates) => Data.Add(coordinates);

    public void Add(double x, double y) => Add(new Coordinates(x, y));

    public void Add(double y)
    {
        double x = Data.Coordinates.Count == 0 ? 0 : Data.Coordinates[^1].X + Period;
        Add(x, y);
    }

    public void Add(IEnumerable<double> ys)
    {
        foreach (double y in ys)
        {
            Add(y);
        }
    }

    public void Add(double[] xs, double[] ys)
    {
        if (xs is null || ys is null)
            throw new ArgumentException($"{nameof(xs)} and {nameof(ys)} must not be null");

        if (xs.Length != ys.Length)
            throw new ArgumentException($"{nameof(xs).Length} and {nameof(ys).Length} must have equal length");

        for (int i = 0; i < xs.Length; i++)
        {
            Add(xs[i], ys[i]);
        }
    }

    public void Add(Coordinates[] coordinates)
    {
        if (coordinates is null)
            throw new ArgumentException($"{coordinates} must not be null");

        for (int i = 0; i < coordinates.Length; i++)
        {
            Add(coordinates[i]);
        }
    }

    public void Clear() => Data.Clear();

    public void UpdateAxisLimits(Plot plot)
    {
        bool firstTimeRenderingData = !Data.WasRendered;

        IAxis xAxis = Rotated ? Axes.YAxis : Axes.XAxis;
        IAxis yAxis = Rotated ? Axes.XAxis : Axes.YAxis;

        CoordinateRange dataRangeX = Data.GetRangeX();
        CoordinateRange viewRangeX = firstTimeRenderingData
            ? dataRangeX
            : xAxis.GetRange().Rectified();

        CoordinateRange newRangeX = AxisManager.GetRangeX(viewRangeX, dataRangeX);

        // Get the Y range only for the newly calculated X range
        CoordinateRange dataRangeY = Data.GetRangeY(newRangeX);
        CoordinateRange viewRangeY = firstTimeRenderingData
            ? dataRangeY
            : yAxis.GetRange().Rectified();

        CoordinateRange newRangeY = AxisManager.GetRangeY(viewRangeY, dataRangeY);

        if (Rotated)
        {
            (newRangeX, newRangeY) = (newRangeY, newRangeX);
        }

        if (InvertX)
        {
            newRangeX = new(newRangeX.Max, newRangeX.Min);
        }

        if (InvertY)
        {
            newRangeY = new(newRangeY.Max, newRangeY.Min);
        }

        AxisLimits newLimits = new(newRangeX, newRangeY);

        plot.Axes.SetLimits(newLimits, Axes.XAxis, Axes.YAxis);
    }

    public bool ManageAxisLimits { get; set; } = true;

    /// <summary>
    /// Automatically expand the axis as needed to ensure the full dataset is visible before each render.
    /// </summary>
    public void ViewFull()
    {
        ManageAxisLimits = true;
        AxisManager = new Full();
        Data.WasRendered = false;
    }

    /// <summary>
    /// Automatically adjust the axis limits to track the newest data as it comes in.
    /// The axis limits will appear to "jump" when new data runs off the screen.
    /// </summary>
    public void ViewJump(double width = 1000, double paddingFraction = .5)
    {
        ManageAxisLimits = true;
        AxisManager = new Slide { Width = width, PaddingFractionX = paddingFraction };
        Data.WasRendered = false;
    }

    /// <summary>
    /// Automatically adjust the axis limits to track the newest data as it comes in.
    /// The axis limits will appear to "slide" continuously as new data is added.
    /// </summary>
    public void ViewSlide(double width = 1000)
    {
        ManageAxisLimits = true;
        AxisManager = new Slide { Width = width, PaddingFractionX = 0 };
        Data.WasRendered = false;
    }
}
