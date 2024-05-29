namespace ScottPlot.Stylers;

/// <summary>
/// Helper methods for setting fonts to all components of a plot
/// </summary>
public class FontStyler(Plot plot)
{
    private readonly Plot Plot = plot;

    /// <summary>
    /// Apply the given font name to all existing plot objects.
    /// Also sets the default font name so this font will be used for plot objects added in the future.
    /// </summary>
    public void Set(string fontName)
    {
        // do nothing if the font can't be located
        using SKTypeface? testTypeface = Fonts.GetTypeface(fontName, bold: false, italic: false);
        if (testTypeface is null)
            return;

        // set default font so future added objects will use it
        Fonts.Default = fontName;

        // title
        Plot.Axes.Title.Label.FontName = fontName;

        // axis labels and ticks
        foreach (IAxis axis in Plot.Axes.GetAxes())
        {
            axis.Label.FontName = fontName;
            axis.TickLabelStyle.FontName = fontName;
        }

        // TODO: also modify plotted text by adding an IHasText interface
    }

    /// <summary>
    /// Detects the best font to apply to every label in the plot based on the characters the they contain.
    /// If the best font for a label cannot be detected, the font last defined by <see cref="Set(string)"/> will be used.
    /// </summary>
    public void Automatic()
    {
        // title
        Plot.Axes.Title.Label.SetBestFont();

        // axis labels and ticks
        foreach (IAxis axis in Plot.Axes.GetAxes())
        {
            axis.Label.SetBestFont();
            axis.TickLabelStyle.SetBestFont();
        }

        Plot.Legend.SetBestFontOnEachRender = true;
    }
}
