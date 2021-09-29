using System.Drawing;

namespace ScottPlot.Themes
{
    public class Blue1 : ITheme
    {
        public Color FigureBackgroundColor => ColorTranslator.FromHtml("#07263b");
        public Color DataBackgroundColor => ColorTranslator.FromHtml("#0b3049");
        public Color FrameColor => ColorTranslator.FromHtml("#145665");
        public Color GridLineColor => ColorTranslator.FromHtml("#0e3d54");
        public Color AxisLabelColor => ColorTranslator.FromHtml("#b5bec5");
        public Color TitleFontColor => ColorTranslator.FromHtml("#d0dae2");
        public Color TickLabelColor => ColorTranslator.FromHtml("#b5bec5");
        public Color TickMajorColor => ColorTranslator.FromHtml("#145665");
        public Color TickMinorColor => ColorTranslator.FromHtml("#145665");
        public string AxisLabelFontName => Drawing.InstalledFont.Default();
        public string TitleFontName => Drawing.InstalledFont.Default();
        public string TickLabelFontName => Drawing.InstalledFont.Default();
    }
}
