namespace ScottPlot.PlotStyles;

public class Dark : PlotStyle
{
    public Dark()
    {
        Palette = new Palettes.Penumbra();
        AxisColor = Color.FromHex("#d7d7d7");
        GridMajorLineColor = Color.FromHex("#404040");
        FigureBackgroundColor = Color.FromHex("#181818");
        DataBackgroundColor = Color.FromHex("#1f1f1f");
        LegendBackgroundColor = Color.FromHex("#404040");
        LegendFontColor = Color.FromHex("#d7d7d7");
        LegendOutlineColor = Color.FromHex("#d7d7d7");
    }
}
