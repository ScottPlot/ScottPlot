namespace ScottPlot.Stylers
{
    public class PlotColorsSettings
    {
        public IPalette Palette = new Palettes.Category10();
        public Color Axes;
        public Color GridMajorLine;
        public Color FigureBackground;
        public Color DataBackGround;

        public Color LegendBackground;
        public Color? LegendFont;
        public Color LegendOutline;
    }
}
