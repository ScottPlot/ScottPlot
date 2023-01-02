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
        Plot.FigureBackground = Color.FromHex("#07263b");
        Plot.DataBackground = Color.FromHex("#0b3049");
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

        Plot.Title.Label.Font.Color = color;
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
}
