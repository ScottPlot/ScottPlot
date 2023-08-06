namespace ScottPlot.Rendering;

/// <summary>
/// Details about a completed render
/// </summary>
public readonly struct RenderDetails
{
    /// <summary>
    /// Size of the plot image in pixel units
    /// </summary>
    public readonly PixelSize FigureSize;

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
    public readonly bool LayoutChanged; // TODO: delete this???

    /// <summary>
    /// Arrangement of all panels
    /// </summary>
    public readonly Layout Layout;

    public RenderDetails(RenderPack rp, (string, TimeSpan)[] actionTimes)
    {
        // TODO: extend actionTimes report individual plottables, axes, etc.

        FigureSize = rp.FigureSize;
        DataRect = rp.DataRect;
        Elapsed = rp.Elapsed;
        Timestamp = DateTime.Now;
        TimedActions = actionTimes;
        AxisLimits = rp.Plot.GetAxisLimits();
        Layout = rp.Layout;

        RenderDetails previous = rp.Plot.RenderManager.LastRenderInfo;

        // TODO: evaluate multi-axis limits (not just the primary axes)
        AxisLimitsChanged = !AxisLimits.Equals(previous.AxisLimits);

        LayoutChanged = !Layout.Equals(previous.Layout);
    }
}
