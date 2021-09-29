using System.Drawing;

namespace ScottPlot.Themes
{
    public class Black : ITheme
    {
        public Color FigureBackgroundColor => Color.Black;
        public Color DataBackgroundColor => Color.Black;
        public Color FrameColor => ColorTranslator.FromHtml("#757575");
        public Color GridLineColor => ColorTranslator.FromHtml("#2d2d2d");
        public Color TitleFontColor => Color.White;
        public Color AxisLabelColor => ColorTranslator.FromHtml("#b9b9ba");
        public Color TickLabelColor => ColorTranslator.FromHtml("#757575");
        public Color TickMajorColor => ColorTranslator.FromHtml("#757575");
        public Color TickMinorColor => ColorTranslator.FromHtml("#757575");
        public string TitleFontName => Drawing.InstalledFont.Default();
        public string AxisLabelFontName => Drawing.InstalledFont.Default();
        public string TickLabelFontName => Drawing.InstalledFont.Default();
    }
}
