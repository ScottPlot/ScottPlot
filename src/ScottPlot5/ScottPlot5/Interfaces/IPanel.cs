namespace ScottPlot;

/// <summary>
/// A panel is a rectangular region outside the data area of a plot.
/// Example panels include axes, colorbars, and titles
/// </summary>
public interface IPanel
{
    /// <summary>
    /// If false, the panel will not be displayed or report any size
    /// </summary>
    public bool IsVisible { get; set; }

    /// <summary>
    /// Disallow the panel to be smaller than this
    /// </summary>
    public float MinimumSize { get; set; }

    /// <summary>
    /// Disallow the panel to be larger than this
    /// </summary>
    public float MaximumSize { get; set; }

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
    /// Draw this panel on a canvas
    /// </summary>
    /// <param name="surface">contains the canvas to draw on</param>
    /// <param name="dataRect">dimensions of the data area (pixel units)</param>
    /// <param name="size">size of this panel (pixel units)</param>
    /// <param name="offset">distance from the edge of this panel to the edge of the data area</param>
    void Render(RenderPack rp, float size, float offset);

    /// <summary>
    /// Enable this to display extra information on the axis to facilitate development
    /// </summary>
    bool ShowDebugInformation { get; set; }

    /// <summary>
    /// Return the rectangle for this panel
    /// </summary>
    PixelRect GetPanelRect(PixelRect dataRect, float size, float offset);
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
