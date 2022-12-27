namespace ScottPlot.LayoutSystem;

public interface IPanel
{
    float Measure();
    Edge Edge { get; }
    void Render(SkiaSharp.SKSurface surface, PixelRect rect);
    bool ShowDebugInformation { get; set; }
}

public static class IPanelExtensions
{
    public static bool IsHorizontal(this IPanel panel) => panel.Edge == Edge.Bottom || panel.Edge == Edge.Top;
    public static bool IsVertical(this IPanel panel) => !panel.IsHorizontal();
}
