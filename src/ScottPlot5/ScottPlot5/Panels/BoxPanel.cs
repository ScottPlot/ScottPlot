using ScottPlot.LayoutSystem;

namespace ScottPlot.Panels;

public class BoxPanel : IPanel
{
    public Edge Edge { get; set; }
    public float Size { get; set; }
    public bool ShowDebugInformation { get; set; } = false;

    public float Measure() => Size;

    public BoxPanel(Edge edge, float size)
    {
        Edge = edge;
        Size = size;
    }

    public PixelRect GetPanelRect(PixelRect rect, float size, float offset)
    {
        return Edge switch
        {
            Edge.Left => new SKRect(rect.Left - Size, rect.Top, Size, rect.Height).ToPixelRect(),
            Edge.Right => new SKRect(rect.Right, rect.Top, Size, rect.Height).ToPixelRect(),
            Edge.Bottom => new SKRect(rect.Left, rect.Bottom, rect.Width, Size).ToPixelRect(),
            Edge.Top => new SKRect(rect.Left, rect.Top - Size, rect.Width, Size).ToPixelRect(),
            _ => throw new NotImplementedException()
        };
    }

    public void Render(SKSurface surface, PixelRect rect, float size, float offset)
    {
        PixelRect panelRect = GetPanelRect(rect, size, offset);
        Drawing.DrawRectangle(surface.Canvas, panelRect, Colors.LightGray);
    }
}
