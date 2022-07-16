using System.Drawing;

namespace ScottPlot.Styles
{
    public class Black : Default
    {
        public override System.Drawing.Color FigureBackgroundColor => System.Drawing.Color.Black;
        public override System.Drawing.Color DataBackgroundColor => System.Drawing.Color.Black;
        public override System.Drawing.Color FrameColor => ColorTranslator.FromHtml("#757575");
        public override System.Drawing.Color GridLineColor => ColorTranslator.FromHtml("#2d2d2d");
        public override System.Drawing.Color TitleFontColor => System.Drawing.Color.White;
        public override System.Drawing.Color AxisLabelColor => ColorTranslator.FromHtml("#b9b9ba");
        public override System.Drawing.Color TickLabelColor => ColorTranslator.FromHtml("#757575");
        public override System.Drawing.Color TickMajorColor => ColorTranslator.FromHtml("#757575");
        public override System.Drawing.Color TickMinorColor => ColorTranslator.FromHtml("#757575");
    }
}
