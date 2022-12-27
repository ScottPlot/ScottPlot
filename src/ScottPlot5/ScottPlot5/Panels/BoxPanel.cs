using ScottPlot.LayoutSystem;
using SkiaSharp;

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

    public void Render(SKSurface surface, PixelRect rect, float size, float offset)
    {
        using var paint = new SKPaint() { Color = SKColors.LightGray };


        if (Edge == Edge.Left)
            surface.Canvas.DrawRect(rect.Left - Size, rect.Top, Size, rect.Height, paint);
        else if (Edge == Edge.Right)
            surface.Canvas.DrawRect(rect.Right, rect.Top, Size, rect.Height, paint);
        else if (Edge == Edge.Bottom)
            surface.Canvas.DrawRect(rect.Left, rect.Bottom, rect.Width, Size, paint);
        else if (Edge == Edge.Top)
            surface.Canvas.DrawRect(rect.Left, rect.Top - Size, rect.Width, Size, paint);
    }
}
