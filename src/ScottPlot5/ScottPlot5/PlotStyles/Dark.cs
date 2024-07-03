namespace ScottPlot.PlotStyles;

public class Dark : PlotStyle
{
    public Dark()
    {
        Palette = new Palettes.Penumbra();
        Axes = Color.FromHex("#d7d7d7");
        GridMajorLine = Color.FromHex("#404040");
        FigureBackground = Color.FromHex("#181818");
        DataBackGround = Color.FromHex("#1f1f1f");
        LegendBackground = Color.FromHex("#404040");
        LegendFont = Color.FromHex("#d7d7d7");
        LegendOutline = Color.FromHex("#d7d7d7");
    }
}
