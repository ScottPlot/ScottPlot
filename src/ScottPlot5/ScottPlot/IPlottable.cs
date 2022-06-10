namespace ScottPlot;

/// <summary>
/// Any object that renders shapes on the canvas using scale information from the axes must implement this interface.
/// </summary>
public interface IPlottable
{
    /// <summary>
    /// Indicates whether or not this plottable will be drawn on the plot.
    /// The calling method will check this variable (it does not need to be checked inside the Render method).
    /// </summary>
    public bool IsVisible { get; set; }

    /// <summary>
    /// Draw the plotable on the given surface
    /// </summary>
    /// <param name="surface">Surface containing the Canvas to be drawn on</param>
    /// <param name="dataRect">Size (in pixels) of the data area</param>
    public void Render(SkiaSharp.SKSurface surface, PixelRect dataRect, HorizontalAxis xAxis, VerticalAxis yAxis);
}