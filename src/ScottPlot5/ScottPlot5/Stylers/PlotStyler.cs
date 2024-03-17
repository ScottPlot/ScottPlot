using ScottPlot.AxisPanels;
using ScottPlot.Grids;
using System.ComponentModel;

namespace ScottPlot.Stylers;

/// <summary>
/// A collection of high-level methods that make it easy to style many components of a plot at once
/// </summary>
public class PlotStyler(Plot plot)
{
    private readonly Plot Plot = plot;

    [Obsolete("This method is deprecated. Assign Plot.FigureBackground.Color instead.", true)]
    public void Background(Color figure, Color data) { }

    /// <summary>
    /// Apply a single color to all components of each axis (label, tick labels, tick marks, and frame)
    /// </summary>
    [Obsolete("Call Plot.Axes.SetColor()")]
    public void ColorAxes(Color color)
    {
        foreach (AxisBase axis in Plot.Axes.GetAxes().OfType<AxisBase>())
        {
            axis.Color(color);
        }

        Plot.Axes.Title.Label.ForeColor = color;
    }

    [Obsolete("Reference Plot.Legend properties directly.", true)]
    public void ColorLegend(Color background, Color foreground, Color border)
    {
        Plot.Legend.BackgroundFill.Color = background;
        Plot.Legend.Font.Color = foreground;
        Plot.Legend.OutlineStyle.Color = border;
    }

    /// <summary>
    /// Set frame thickness for each side of the plot
    /// </summary>
    public void AxisFrame(float left, float right, float bottom, float top)
    {
        Plot.Axes.Left.FrameLineStyle.Width = left;
        Plot.Axes.Right.FrameLineStyle.Width = right;
        Plot.Axes.Bottom.FrameLineStyle.Width = bottom;
        Plot.Axes.Top.FrameLineStyle.Width = top;
    }

    /// <summary>
    /// Apply the given font name to all existing plot objects.
    /// Also sets the default font name so this font will be used for plot objects added in the future.
    /// </summary>
    public void SetFont(string fontName)
    {
        fontName = Fonts.Exists(fontName) ? fontName : Fonts.Default;

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

        // TODO: also modify tick labels
        // TODO: also modify plotted text
    }

    [Obsolete("use SetBestFonts()", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public bool SetFontFromText(string text)
    {
        throw new InvalidOperationException();
    }

    /// <summary>
    /// Detects the best font to apply to every label in the plot based on the characters the they contain.
    /// If the best font for a label cannot be detected, the font last defined by <see cref="SetFont(string)"/> will be used.
    /// </summary>
    public void SetBestFonts()
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

        // TODO: also modify plotted text by adding an IHasText interface
    }

    /// <summary>
    /// Reset colors and palette do a dark mode style
    /// </summary>
    public void DarkMode()
    {
        Plot.Add.Palette = new Palettes.Penumbra();

        ColorAxes(Color.FromHex("#d7d7d7"));

        Plot.Grid.LineColor = Color.FromHex("#404040");
        Plot.FigureBackground.Color = Color.FromHex("#181818");
        Plot.DataBackground.Color = Color.FromHex("#1f1f1f");

        Plot.Legend.BackgroundFill.Color = Color.FromHex("#404040");
        Plot.Legend.Font.Color = Color.FromHex("#d7d7d7");
        Plot.Legend.OutlineStyle.Color = Color.FromHex("#d7d7d7");
    }
}
