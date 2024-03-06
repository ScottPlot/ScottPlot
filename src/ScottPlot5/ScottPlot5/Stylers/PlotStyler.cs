using ScottPlot.AxisPanels;
using ScottPlot.Grids;
using ScottPlot.Legends;
using System.ComponentModel;

namespace ScottPlot.Stylers;

/// <summary>
/// A collection of high-level methods that make it easy to style many components of a plot at once
/// </summary>
public class PlotStyler
{
    private readonly Plot Plot;

    public PlotStyler(Plot plot)
    {
        Plot = plot;
    }

    /// <summary>
    /// Apply background colors to the figure and data areas
    /// </summary>
    public void Background(Color figure, Color data)
    {
        Plot.FigureBackground = figure;
        Plot.DataBackground = data;
    }

    /// <summary>
    /// Apply a background image to the data frame
    /// </summary>
    public void DataBackgroundImage(string filePath, ImageScalingStyle scalingStyle = ImageScalingStyle.FillRetainAspect)
    {
        Plot.DataBackgroundImage = SKBitmap.Decode(filePath);
        Plot.DataBackgroundScalingStyle = scalingStyle;
    }

    /// <summary>
    /// Apply a background image to the data frame
    /// </summary>
    public void DataBackgroundImage(byte[] imageByteData, ImageScalingStyle scalingStyle = ImageScalingStyle.FillRetainAspect)
    {
        Plot.DataBackgroundImage = SKBitmap.Decode(imageByteData);
        Plot.DataBackgroundScalingStyle = scalingStyle;
    }

    /// <summary>
    /// Apply a color to the data frame background image
    /// </summary>
    public void ColorDataBackgroundImage(Color color)
    {
        Plot.DataBackgroundImageColor = color;
    }

    /// <summary>
    /// Apply a single color to all components of each axis (label, tick labels, tick marks, and frame)
    /// </summary>
    public void ColorAxes(Color color)
    {
        foreach (AxisBase axis in Plot.Axes.GetAxes().OfType<AxisBase>())
        {
            axis.Color(color);
        }

        Plot.Axes.Title.Label.ForeColor = color;
    }

    /// <summary>
    /// Apply a color to all currently visible grids
    /// </summary>
    public void ColorGrids(Color majorColor)
    {
        foreach (DefaultGrid grid in Plot.Axes.Grids.OfType<DefaultGrid>())
        {
            grid.MajorLineStyle.Color = majorColor;
        }
    }

    /// <summary>
    /// Apply a color to all currently visible grids
    /// </summary>
    public void ColorGrids(Color majorColor, Color minorColor)
    {
        foreach (DefaultGrid grid in Plot.Axes.Grids.OfType<DefaultGrid>())
        {
            grid.MajorLineStyle.Color = majorColor;
            grid.MinorLineStyle.Color = minorColor;
        }
    }

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
        }

        // TODO: also modify tick labels
        // TODO: also modify plotted text
    }

    /// <summary>
    /// Reset colors and palette do a dark mode style
    /// </summary>
    public void DarkMode()
    {
        Plot.Add.Palette = new Palettes.Penumbra();

        ColorAxes(Color.FromHex("#d7d7d7"));
        ColorGrids(Color.FromHex("#404040"));
        Background(
            figure: Color.FromHex("#181818"),
            data: Color.FromHex("#1f1f1f"));
        ColorLegend(
            background: Color.FromHex("#404040"),
            foreground: Color.FromHex("#d7d7d7"),
            border: Color.FromHex("#d7d7d7"));
    }
}
