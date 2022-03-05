using System.Drawing;

namespace ScottPlot.Styles
{
    internal class Blue2 : Default
    {
        public override Color FigureBackgroundColor => ColorTranslator.FromHtml("#1b2138");
        public override Color DataBackgroundColor => ColorTranslator.FromHtml("#252c48");
        public override Color FrameColor => ColorTranslator.FromHtml("#bbbdc4");
        public override Color GridLineColor => ColorTranslator.FromHtml("#2c334e");
        public override Color AxisLabelColor => ColorTranslator.FromHtml("#bbbdc4");
        public override Color TitleFontColor => ColorTranslator.FromHtml("#d8dbe3");
        public override Color TickLabelColor => ColorTranslator.FromHtml("#bbbdc4");
        public override Color TickMajorColor => ColorTranslator.FromHtml("#bbbdc4");
        public override Color TickMinorColor => ColorTranslator.FromHtml("#bbbdc4");
    }
}
