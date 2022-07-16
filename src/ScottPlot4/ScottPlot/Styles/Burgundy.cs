using System.Drawing;

namespace ScottPlot.Styles
{
    public class Burgundy : Default
    {
        public override System.Drawing.Color FigureBackgroundColor => ColorTranslator.FromHtml("#ffffff");
        public override System.Drawing.Color DataBackgroundColor => ColorTranslator.FromHtml("#fffdfd");
        public override System.Drawing.Color FrameColor => ColorTranslator.FromHtml("#560013");
        public override System.Drawing.Color GridLineColor => ColorTranslator.FromHtml("#ffdae3");
        public override System.Drawing.Color AxisLabelColor => ColorTranslator.FromHtml("#5e0015");
        public override System.Drawing.Color TitleFontColor => ColorTranslator.FromHtml("#560013");
        public override System.Drawing.Color TickLabelColor => ColorTranslator.FromHtml("#5e0015");
        public override System.Drawing.Color TickMajorColor => ColorTranslator.FromHtml("#560013");
        public override System.Drawing.Color TickMinorColor => ColorTranslator.FromHtml("#560013");
    }
}
