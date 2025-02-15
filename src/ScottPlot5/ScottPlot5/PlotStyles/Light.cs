namespace ScottPlot.PlotStyles;

public class Light : PlotStyle
{
    public Light()
    {
        Palette = new Palettes.Category10();
        AxisColor = Colors.Black;
        GridMajorLineColor = Colors.Black.WithOpacity(.1);
        FigureBackgroundColor = Colors.White;
        DataBackgroundColor = Colors.White;
        LegendBackgroundColor = Colors.White;
        LegendFontColor = Colors.Black;
        LegendOutlineColor = Colors.Black;
    }
}
