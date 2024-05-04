namespace ScottPlot.Stylers;

/// <summary>
/// Helper methods for applying common styling options to plots
/// </summary>
public class PlotStyler(Plot plot)
{
    private readonly Plot Plot = plot;

    [Obsolete("This method is deprecated. Assign Plot.FigureBackground.Color instead.", true)]
    public void Background(Color figure, Color data) { }

    [Obsolete("This method is deprecated. Call Plot.Axes.Color() instead.", true)]
    public void ColorAxes(Color color) { }

    [Obsolete("Reference Plot.Legend properties directly.", true)]
    public void ColorLegend(Color background, Color foreground, Color border) { }

    [Obsolete("This method is deprecated. Call Plot.Axes.Frame() methods instead.", true)]
    public void AxisFrame(float left, float right, float bottom, float top) { }

    [Obsolete("This method is deprecated. Call Plot.Font.Set() instead.", true)]
    public void SetFont(string fontName) { }

    [Obsolete("This method is deprecated. Call Plot.Font.Automatic() instead.", true)]
    public void SetBestFonts() { }

    /// <summary>
    /// Reset colors and palette do a dark mode style
    /// </summary>
    public void DarkMode()
    {
        Plot.Add.Palette = new Palettes.Penumbra();

        Plot.Axes.Color(Color.FromHex("#d7d7d7"));

        Plot.Grid.MajorLineColor = Color.FromHex("#404040");
        Plot.FigureBackground.Color = Color.FromHex("#181818");
        Plot.DataBackground.Color = Color.FromHex("#1f1f1f");

        Plot.Legend.BackgroundColor = Color.FromHex("#404040");
        Plot.Legend.FontColor = Color.FromHex("#d7d7d7");
        Plot.Legend.OutlineColor = Color.FromHex("#d7d7d7");
    }
}
