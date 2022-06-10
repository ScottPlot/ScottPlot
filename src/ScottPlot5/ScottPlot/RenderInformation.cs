namespace ScottPlot;

/// <summary>
/// Stores information about a render for debugging or later retrieval.
/// </summary>
public struct RenderInformation
{
    public TimeSpan ElapsedLayout;
    public TimeSpan ElapsedRender;
    public PixelRect FigureRect;
    public PixelRect DataRect;
    public bool RenderComplete => ElapsedRender.TotalMilliseconds > 0;
    public double ElapsedMilliseconds => ElapsedLayout.TotalMilliseconds + ElapsedRender.TotalMilliseconds;
}
