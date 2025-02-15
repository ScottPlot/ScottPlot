namespace ScottPlot.Stylers;

[Obsolete("This class is deprecated. See the ScottPlot Cookbook for styling information.", true)]
public class PlotStyler()
{
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

    [Obsolete("This method is deprecated. Call Plot.SetStyle() instead.", true)]
    public void DarkMode() { }
}
