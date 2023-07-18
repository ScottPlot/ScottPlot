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

    public RenderDetails(RenderPack rp)
    {
        FigureRect = new PixelRect(0, 0, rp.FigureSize.Width, rp.FigureSize.Height);
        DataRect = rp.DataRect;
        Elapsed = rp.Elapsed;
        Timestamp = DateTime.Now;
    }
}
