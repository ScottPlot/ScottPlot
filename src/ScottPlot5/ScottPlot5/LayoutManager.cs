using ScottPlot.LayoutEngines;

namespace ScottPlot;

/// <summary>
/// A collection of methods for making common adjustments to plot layouts
/// </summary>
public class LayoutManager
{
    private readonly Plot Plot;

    public ILayoutEngine LayoutEngine { get; set; } = new Automatic();

    public LayoutManager(Plot plot)
    {
        Plot = plot;
    }

    /// <summary>
    /// Automatically resize the layout on each render to achieve the best fit
    /// </summary>
    public void Default()
    {
        LayoutEngine = new Automatic();
    }

    /// <summary>
    /// Apply a fixed layout using the given rectangle to define the data area
    /// </summary>
    public void Fixed(PixelRect dataRect)
    {
        LayoutEngine = new FixedDataArea(dataRect);
    }

    /// <summary>
    /// Apply a fixed layout using the given padding to define space between the
    /// data area and the edge of the figure
    /// </summary>
    public void Fixed(PixelPadding padding)
    {
        LayoutEngine = new FixedPadding(padding);
    }

    /// <summary>
    /// Disable visibility of all axes and titles so the data area fills the entire figure
    /// </summary>
    public void Frameless()
    {
        Plot.Axes.Frameless();
    }
}
