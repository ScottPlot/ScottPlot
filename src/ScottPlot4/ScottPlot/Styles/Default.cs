using System.Drawing;

namespace ScottPlot.Styles
{
    public class Default : IStyle
    {
        public virtual System.Drawing.Color FigureBackgroundColor => System.Drawing.Color.White;
        public virtual System.Drawing.Color DataBackgroundColor => System.Drawing.Color.White;
        public virtual System.Drawing.Color GridLineColor => ColorTranslator.FromHtml("#efefef");
        public virtual System.Drawing.Color FrameColor => System.Drawing.Color.Black;
        public virtual System.Drawing.Color TitleFontColor => System.Drawing.Color.Black;
        public virtual System.Drawing.Color AxisLabelColor => System.Drawing.Color.Black;
        public virtual System.Drawing.Color TickLabelColor => System.Drawing.Color.Black;
        public virtual System.Drawing.Color TickMajorColor => System.Drawing.Color.Black;
        public virtual System.Drawing.Color TickMinorColor => System.Drawing.Color.Black;

        public virtual string TitleFontName => Drawing.InstalledFont.Default();
        public virtual string AxisLabelFontName => Drawing.InstalledFont.Default();
        public virtual string TickLabelFontName => Drawing.InstalledFont.Default();
    }
}
