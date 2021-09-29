using System.Drawing;

namespace ScottPlot.Styles
{
    public class Default : IStyle
    {
        public virtual Color FigureBackgroundColor => Color.White;
        public virtual Color DataBackgroundColor => Color.White;
        public virtual Color GridLineColor => ColorTranslator.FromHtml("#efefef");
        public virtual Color FrameColor => Color.Black;
        public virtual Color TitleFontColor => Color.Black;
        public virtual Color AxisLabelColor => Color.Black;
        public virtual Color TickLabelColor => Color.Black;
        public virtual Color TickMajorColor => Color.Black;
        public virtual Color TickMinorColor => Color.Black;

        public virtual string TitleFontName => Drawing.InstalledFont.Default();
        public virtual string AxisLabelFontName => Drawing.InstalledFont.Default();
        public virtual string TickLabelFontName => Drawing.InstalledFont.Default();
    }
}
