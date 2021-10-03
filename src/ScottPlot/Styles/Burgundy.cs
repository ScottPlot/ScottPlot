using System.Drawing;

namespace ScottPlot.Styles
{
    public class Burgundy : Default
    {
        public override Color FigureBackgroundColor => ColorTranslator.FromHtml("#ffffff");
        public override Color DataBackgroundColor => ColorTranslator.FromHtml("#fffdfd");
        public override Color FrameColor => ColorTranslator.FromHtml("#560013");
        public override Color GridLineColor => ColorTranslator.FromHtml("#ffdae3");
        public override Color AxisLabelColor => ColorTranslator.FromHtml("#5e0015");
        public override Color TitleFontColor => ColorTranslator.FromHtml("#560013");
        public override Color TickLabelColor => ColorTranslator.FromHtml("#5e0015");
        public override Color TickMajorColor => ColorTranslator.FromHtml("#560013");
        public override Color TickMinorColor => ColorTranslator.FromHtml("#560013");
    }
}
