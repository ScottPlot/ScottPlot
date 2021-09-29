using System.Drawing;

namespace ScottPlot.Themes
{
    public class Gray1 : ITheme
    {
        public Color FigureBackgroundColor => ColorTranslator.FromHtml("#31363a");
        public Color DataBackgroundColor => ColorTranslator.FromHtml("#3a4149");
        public Color FrameColor => ColorTranslator.FromHtml("#757a80");
        public Color GridLineColor => ColorTranslator.FromHtml("#444b52");
        public Color AxisLabelColor => ColorTranslator.FromHtml("#d6d7d8");
        public Color TitleFontColor => ColorTranslator.FromHtml("#FFFFFF");
        public Color TickLabelColor => ColorTranslator.FromHtml("#757a80");
        public Color TickMajorColor => ColorTranslator.FromHtml("#757a80");
        public Color TickMinorColor => ColorTranslator.FromHtml("#757a80");
        public string AxisLabelFontName => Drawing.InstalledFont.Default();
        public string TitleFontName => Drawing.InstalledFont.Default();
        public string TickLabelFontName => Drawing.InstalledFont.Default();
    }
}
