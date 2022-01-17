using Microsoft.Maui.Graphics;

namespace ScottPlot;

/// <summary>
/// This class holds styling options for the plot.
/// These options may be exposed to plottables in case they wish to use these colors too.
/// </summary>
public class PlotStyle
{
    public Color FigureBackgroundColor = Color.Parse("#003366");

    public Color DataBackgroundColor = Color.Parse("#006699");
    public Color DataBorderColor = Colors.Black;
}