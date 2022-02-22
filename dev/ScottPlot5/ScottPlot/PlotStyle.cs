using Microsoft.Maui.Graphics;

namespace ScottPlot;

/// <summary>
/// This class holds styling options for the plot.
/// These options may be exposed to plottables in case they wish to use these colors too.
/// </summary>
public class PlotStyle
{
    //NOTE: Eventually default font information can be stored here

    public Color FigureBackgroundColor = Colors.White;

    public Color DataBackgroundColor = Colors.White;

    public Color SpineColor = Colors.Black;

    public readonly Palette Palette = Palettes.Default;
}