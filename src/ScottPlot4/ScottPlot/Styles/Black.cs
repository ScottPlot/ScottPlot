using System.Drawing;

namespace ScottPlot.Styles
{
    public class Black : Default
    {
        public override Color FigureBackgroundColor => Color.Black;
        public override Color DataBackgroundColor => Color.Black;
        public override Color FrameColor => ColorTranslator.FromHtml("#757575");
        public override Color GridLineColor => ColorTranslator.FromHtml("#2d2d2d");
        public override Color TitleFontColor => Color.White;
        public override Color AxisLabelColor => ColorTranslator.FromHtml("#b9b9ba");
        public override Color TickLabelColor => ColorTranslator.FromHtml("#757575");
        public override Color TickMajorColor => ColorTranslator.FromHtml("#757575");
        public override Color TickMinorColor => ColorTranslator.FromHtml("#757575");
    }
}
