using System.Drawing;

namespace ScottPlot.Themes
{
    /// <summary>
    /// A theme describes a collection of colors and fonts that can be used to style a plot
    /// </summary>
    public interface ITheme
    {
        Color FigureBackgroundColor { get; }
        Color DataBackgroundColor { get; }
        Color FrameColor { get; }
        Color GridLineColor { get; }
        Color AxisLabelColor { get; }
        Color TitleFontColor { get; }
        Color TickLabelColor { get; }
        Color TickMajorColor { get; }
        Color TickMinorColor { get; }

        string AxisLabelFontName { get; }
        string TitleFontName { get; }
        string TickLabelFontName { get; }
    }
}
