namespace ScottPlot.LayoutSystem;

public interface IPanel
{
    /// <summary>
    /// Return the size (in pixels) of the panel in the dimension perpendicular to the edge it lays on
    /// </summary>
    /// <returns></returns>
    float Measure();

    /// <summary>
    /// Indicates which edge of the data rectangle this panel lays on
    /// </summary>
    Edge Edge { get; }

    /// <summary>
    /// Draw this panel on the given surface knowing the data area and the offset this panel is to be drawn from it
    /// </summary>
    void Render(SkiaSharp.SKSurface surface, PixelRect dataRect, float offset);

    /// <summary>
    /// Enable this to display extra information on the axis to facilitate development
    /// </summary>
    bool ShowDebugInformation { get; set; }
}

public static class IPanelExtensions
{
    /// <summary>
    /// Returns true for X axes (bottom and top)
    /// </summary>
    public static bool IsHorizontal(this IPanel panel) => panel.Edge == Edge.Bottom || panel.Edge == Edge.Top;

    /// <summary>
    /// Returns true for Y axes (left and right)
    /// </summary>
    public static bool IsVertical(this IPanel panel) => !panel.IsHorizontal();
}
