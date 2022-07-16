using System.Drawing;

namespace ScottPlot.Styles
{
    public class Gray1 : Default
    {
        public override System.Drawing.Color FigureBackgroundColor => ColorTranslator.FromHtml("#31363a");
        public override System.Drawing.Color DataBackgroundColor => ColorTranslator.FromHtml("#3a4149");
        public override System.Drawing.Color FrameColor => ColorTranslator.FromHtml("#757a80");
        public override System.Drawing.Color GridLineColor => ColorTranslator.FromHtml("#444b52");
        public override System.Drawing.Color AxisLabelColor => ColorTranslator.FromHtml("#d6d7d8");
        public override System.Drawing.Color TitleFontColor => ColorTranslator.FromHtml("#FFFFFF");
        public override System.Drawing.Color TickLabelColor => ColorTranslator.FromHtml("#757a80");
        public override System.Drawing.Color TickMajorColor => ColorTranslator.FromHtml("#757a80");
        public override System.Drawing.Color TickMinorColor => ColorTranslator.FromHtml("#757a80");
    }
}
