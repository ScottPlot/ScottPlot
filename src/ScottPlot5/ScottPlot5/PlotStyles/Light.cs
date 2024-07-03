namespace ScottPlot.PlotStyles;

public class Light : PlotStyle
{
    public Light()
    {
        Palette = new Palettes.Category10();
        Axes = Colors.Black;
        GridMajorLine = Colors.Black.WithOpacity(.1);
        FigureBackground = Colors.White;
        DataBackGround = Colors.White;
        LegendBackground = Colors.White;
        LegendFont = Colors.Black;
        LegendOutline = Colors.Black;
    }
}
