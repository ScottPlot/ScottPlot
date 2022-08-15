namespace ScottPlot.Rendering;

/// <summary>
/// Detaiols about a completed render
/// </summary>
public struct RenderDetails
{
    public readonly PixelRect FigureRect;
    public readonly PixelRect DataRect;
    public readonly TimeSpan Elapsed;
    public readonly DateTime Timestamp;

    public RenderDetails(PixelRect figureArea, PixelRect dataArea, TimeSpan elapsed)
    {
        FigureRect = figureArea;
        DataRect = dataArea;
        Elapsed = elapsed;
        Timestamp = DateTime.Now;
    }
}
