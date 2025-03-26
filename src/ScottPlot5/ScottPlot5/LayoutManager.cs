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
    /// Helper method to set visibility of all axes and title panels.
    /// Hiding all panels allows the data area to the extend to the edge of the figure.
    /// This method hides the title, but call "Plot.Title()" to re-enable it.
    /// </summary>
    public void Frameless(bool hideAllPanels = true)
    {
        Plot.Axes.Frameless(hideAllPanels);
    }
}
