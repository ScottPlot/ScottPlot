using ScottPlot.Axes;

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

    // * Typically the render method will contain a ClipRect() call
    // * Clipping is reset automatically after every plottable is rendered

    /// <summary>
    /// Draw the plotable on the given surface.
    /// </summary>
    /// <param name="surface">Surface containing the Canvas to be drawn on</param>
    /// <param name="dataRect">Size (in pixels) of the data area</param>
    public void Render(SkiaSharp.SKSurface surface, PixelRect dataRect);

    // * If these are null on first render, set them with the default axes.
    // * Plottables that use these must perform a null check once at the top of the render method.
    // * Plottables that don't need axes can leave these null.

    public IXAxis? XAxis { get; set; }

    public IYAxis? YAxis { get; set; }

    public AxisLimits GetAxisLimits();
}
