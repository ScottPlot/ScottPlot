using System.Drawing;

namespace ScottPlot.Styles
{
    /// <summary>
    /// A theme describes a collection of colors and fonts that can be used to style a plot
    /// </summary>
    public interface IStyle
    {
        System.Drawing.Color FigureBackgroundColor { get; }
        System.Drawing.Color DataBackgroundColor { get; }
        System.Drawing.Color FrameColor { get; }
        System.Drawing.Color GridLineColor { get; }
        System.Drawing.Color AxisLabelColor { get; }
        System.Drawing.Color TitleFontColor { get; }
        System.Drawing.Color TickLabelColor { get; }
        System.Drawing.Color TickMajorColor { get; }
        System.Drawing.Color TickMinorColor { get; }

        string AxisLabelFontName { get; }
        string TitleFontName { get; }
        string TickLabelFontName { get; }
    }
}
