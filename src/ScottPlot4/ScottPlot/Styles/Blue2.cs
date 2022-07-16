using System.Drawing;

namespace ScottPlot.Styles
{
    internal class Blue2 : Default
    {
        public override System.Drawing.Color FigureBackgroundColor => ColorTranslator.FromHtml("#1b2138");
        public override System.Drawing.Color DataBackgroundColor => ColorTranslator.FromHtml("#252c48");
        public override System.Drawing.Color FrameColor => ColorTranslator.FromHtml("#bbbdc4");
        public override System.Drawing.Color GridLineColor => ColorTranslator.FromHtml("#2c334e");
        public override System.Drawing.Color AxisLabelColor => ColorTranslator.FromHtml("#bbbdc4");
        public override System.Drawing.Color TitleFontColor => ColorTranslator.FromHtml("#d8dbe3");
        public override System.Drawing.Color TickLabelColor => ColorTranslator.FromHtml("#bbbdc4");
        public override System.Drawing.Color TickMajorColor => ColorTranslator.FromHtml("#bbbdc4");
        public override System.Drawing.Color TickMinorColor => ColorTranslator.FromHtml("#bbbdc4");
    }
}
