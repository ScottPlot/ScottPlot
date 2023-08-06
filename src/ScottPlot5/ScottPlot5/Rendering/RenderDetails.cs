namespace ScottPlot.Rendering;

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
    /// Axis limits for this render
    /// </summary>
    public readonly AxisLimits AxisLimits;

    /// <summary>
    /// Indicates whether the axis limits of this render are different
    /// from those of the previous render.
    /// </summary>
    public readonly bool AxisLimitsChanged;

    /// <summary>
    /// Indicates whether the pixel dimensions of this render are different
    /// from those of the previous render.
    /// </summary>
    public readonly bool LayoutChanged;

    public RenderDetails(RenderPack rp, (string, TimeSpan)[] actionTimes)
    {
        // TODO: extend actionTimes report individual plottables, axes, etc.

        FigureRect = new PixelRect(0, 0, rp.FigureSize.Width, rp.FigureSize.Height);
        DataRect = rp.DataRect;
        Elapsed = rp.Elapsed;
        Timestamp = DateTime.Now;
        TimedActions = actionTimes;
        AxisLimits = rp.Plot.GetAxisLimits();

        RenderDetails previous = rp.Plot.RenderManager.LastRenderInfo;

        // TODO: evaluate multi-axis limits (not just the primary axes)
        AxisLimitsChanged = !AxisLimits.Equals(previous.AxisLimits);

        bool figureRectChanged = !FigureRect.Equals(previous.FigureRect);
        bool dataRectChanged = !DataRect.Equals(previous.DataRect);
        LayoutChanged = figureRectChanged || dataRectChanged;
    }
}
