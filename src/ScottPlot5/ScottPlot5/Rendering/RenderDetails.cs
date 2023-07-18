namespace ScottPlot.Rendering;

/// <summary>
/// Details about a completed render
/// </summary>
public struct RenderDetails
{
    public readonly PixelRect FigureRect;
    public readonly PixelRect DataRect;
    public readonly TimeSpan Elapsed;
    public readonly DateTime Timestamp;
    public readonly (string, TimeSpan)[] TimedActions; // TODO: make this report individual plottables, axes, etc.

    public RenderDetails(RenderPack rp, (string, TimeSpan)[] actionTimes)
    {
        FigureRect = new PixelRect(0, 0, rp.FigureSize.Width, rp.FigureSize.Height);
        DataRect = rp.DataRect;
        Elapsed = rp.Elapsed;
        Timestamp = DateTime.Now;
        TimedActions = actionTimes;
    }
}
