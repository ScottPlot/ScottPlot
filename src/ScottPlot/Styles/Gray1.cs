using System.Drawing;

namespace ScottPlot.Styles
{
    public class Gray1 : Default
    {
        public override Color FigureBackgroundColor => ColorTranslator.FromHtml("#31363a");
        public override Color DataBackgroundColor => ColorTranslator.FromHtml("#3a4149");
        public override Color FrameColor => ColorTranslator.FromHtml("#757a80");
        public override Color GridLineColor => ColorTranslator.FromHtml("#444b52");
        public override Color AxisLabelColor => ColorTranslator.FromHtml("#d6d7d8");
        public override Color TitleFontColor => ColorTranslator.FromHtml("#FFFFFF");
        public override Color TickLabelColor => ColorTranslator.FromHtml("#757a80");
        public override Color TickMajorColor => ColorTranslator.FromHtml("#757a80");
        public override Color TickMinorColor => ColorTranslator.FromHtml("#757a80");
    }
}
