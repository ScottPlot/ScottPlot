namespace ScottPlot;

/// <summary>
/// Details about a completed render
/// </summary>
public readonly struct RenderDetails
{
    /// <summary>
    /// Size of the plot image in pixel units
    /// </summary>
    public readonly PixelRect FigureRect;

    /// <summary>
    /// Size of the data area of the plot in pixel units
    /// </summary>
    public readonly PixelRect DataRect;

    /// <summary>
    /// Distance between the data area and the edge of the figure
    /// </summary>
    public readonly PixelPadding Padding;

    /// <summary>
    /// Total time required to render this image
    /// </summary>
    public readonly TimeSpan Elapsed;

    /// <summary>
    /// Time the render was completed
    /// </summary>
    public readonly DateTime Timestamp;

    /// <summary>
    /// Each step of the render and how long it took to execute
    /// </summary>
    public readonly (string, TimeSpan)[] TimedActions;

    /// <summary>
    /// Axis limits of the primary axes for this render
    /// </summary>
    public readonly AxisLimits AxisLimits;

    /// <summary>
    /// Axis limits for every axis
    /// </summary>
    public readonly Dictionary<IAxis, CoordinateRange> AxisLimitsByAxis;

    /// <summary>
    /// Indicates whether the axis view (coordinate units) of this render differs from the previous
    /// </summary>
    public readonly bool AxisLimitsChanged { get; }

    /// <summary>
    /// Indicates whether the size (pixels) of this render differs from the previous
    /// </summary>
    public readonly bool SizeChanged { get; }

    /// <summary>
    /// Arrangement of all panels
    /// </summary>
    public readonly Layout Layout;

    /// <summary>
    /// The number of total renders including this one
    /// </summary>
    public readonly int Count;

    public double PxPerUnitX => DataRect.Width / AxisLimits.HorizontalSpan;
    public double PxPerUnitY => DataRect.Height / AxisLimits.VerticalSpan;
    public double UnitsPerPxX => AxisLimits.HorizontalSpan / DataRect.Width;
    public double UnitsPerPxY => AxisLimits.VerticalSpan / DataRect.Height;

    public RenderDetails(RenderPack rp, (string, TimeSpan)[] actionTimes, RenderDetails lastRender)
    {
        // TODO: extend actionTimes report individual plottables, axes, etc.
        FigureRect = rp.FigureRect;
        DataRect = rp.DataRect;
        Padding = new PixelPadding(rp.FigureRect.Size, rp.DataRect);
        Elapsed = rp.Elapsed;
        Timestamp = DateTime.Now;
        TimedActions = actionTimes;
        AxisLimits = rp.Plot.Axes.GetLimits();
        AxisLimitsByAxis = rp.Plot.Axes.GetAxes().ToDictionary(x => x, x => x.Range);
        Layout = rp.Layout;
        Count = lastRender.Count + 1;

        // TODO: evaluate multi-axis limits (not just the primary axes)
        AxisLimitsChanged = !AxisLimits.Equals(lastRender.AxisLimits);
        SizeChanged = !DataRect.Equals(lastRender.DataRect);
    }
}
