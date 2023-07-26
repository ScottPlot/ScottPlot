using ScottPlot.Axis;
using ScottPlot.Axis.StandardAxes;
using ScottPlot.Grids;

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
    /// Apply a single color to all components of each axis (label, tick labels, tick marks, and frame)
    /// </summary>
    public void ColorAxes(Color color)
    {
        foreach (AxisBase axis in Plot.GetStandardAxes())
        {
            axis.Label.Font.Color = color;
            axis.FrameLineStyle.Color = color;
            axis.MajorTickColor = color;
            axis.MinorTickColor = color;
        }

        Plot.TitlePanel.Label.Font.Color = color;
    }

    /// <summary>
    /// Apply a color to all currently visible grids
    /// </summary>
    public void ColorGrids(Color majorColor)
    {
        foreach (DefaultGrid grid in Plot.Grids.OfType<DefaultGrid>())
        {
            grid.MajorLineStyle.Color = majorColor;
        }
    }

    /// <summary>
    /// Apply a color to all currently visible grids
    /// </summary>
    public void ColorGrids(Color majorColor, Color minorColor)
    {
        foreach (DefaultGrid grid in Plot.Grids.OfType<DefaultGrid>())
        {
            grid.MajorLineStyle.Color = majorColor;
            grid.MinorLineStyle.Color = minorColor;
        }
    }

    /// <summary>
    /// Set frame thickness for each side of the plot
    /// </summary>
    public void AxisFrame(float left, float right, float bottom, float top)
    {
        AxisBase[] axes = Plot.GetStandardAxes();
        axes[0].FrameLineStyle.Width = left;
        axes[1].FrameLineStyle.Width = right;
        axes[2].FrameLineStyle.Width = bottom;
        axes[3].FrameLineStyle.Width = top;
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
        Plot.TitlePanel.Label.Font.Name = fontName;

        // axis labels and ticks
        foreach (IAxis axis in Plot.GetAllAxes())
        {
            axis.Label.Font.Name = fontName;
            axis.TickFont.Name = fontName;
        }
    }

    /// <summary>
    /// Calls <see cref="SetFont(string)"/> using the system font best suited
    /// to display the first character in the given string.
    /// </summary>
    public bool SetFontFromText(string text)
    {
        string? fontName = Fonts.Detect(text);
        bool success = !string.IsNullOrEmpty(fontName);
        SetFont(fontName ?? Fonts.Default);
        return success;
    }
}
