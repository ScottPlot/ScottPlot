using ScottPlot.AxisLimitManagers;

namespace ScottPlot.Plottables.Experimental;

public class DataStreamer2(IDataStreamer2Source dataSource) : IPlottable, IManagesAxisLimits, IHasLine, IHasMarker, IHasLegendText
{
    public IDataStreamer2Source Data { get; } = dataSource;

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

    [Obsolete("use LegendText")]
    public string Label { get => LegendText; set => LegendText = value; }
    public string LegendText { get; set; } = string.Empty;

    public IEnumerable<LegendItem> LegendItems => LegendItem.Single(LegendText, LineStyle, MarkerStyle);

    public AxisLimits GetAxisLimits()
    {
        CoordinateRange rangeX = Data.GetRangeX();
        CoordinateRange rangeY = Data.GetRangeY(rangeX);

        return Rotated
            ? new AxisLimits(rangeY, rangeX)
            : new AxisLimits(rangeX, rangeY);
    }

    public DataPoint GetNearest(Coordinates location, RenderDetails renderInfo, float maxDistance = 15) =>
        Data.GetNearest(location, renderInfo, maxDistance);

    public virtual void Render(RenderPack rp)
    {
        Pixel[] markerPixels = Rotated ? Data.GetPixelsToDrawVertically(rp, Axes, ConnectStyle)
                                       : Data.GetPixelsToDrawHorizontally(rp, Axes, ConnectStyle);

        Pixel[] linePixels = ConnectStyle switch
        {
            ConnectStyle.Straight => markerPixels,
            ConnectStyle.StepHorizontal => Scatter.GetStepDisplayPixels(markerPixels, true),
            ConnectStyle.StepVertical => Scatter.GetStepDisplayPixels(markerPixels, false),
            _ => throw new NotImplementedException($"unsupported {nameof(ConnectStyle)}: {ConnectStyle}"),
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
        const double Period = 1.0;
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

        if (Axes.XAxis.IsInverted())
        {
            newRangeX = new(newRangeX.Max, newRangeX.Min);
        }

        if (Axes.YAxis.IsInverted())
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
