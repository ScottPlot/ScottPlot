using System.Drawing;

namespace ScottPlot.Styles
{
    internal class Seaborn : Default
    {
        public override System.Drawing.Color DataBackgroundColor => ColorTranslator.FromHtml("#eaeaf2");
        public override System.Drawing.Color FrameColor => System.Drawing.Color.Transparent;
        public override System.Drawing.Color GridLineColor => System.Drawing.Color.White;
        public override System.Drawing.Color AxisLabelColor => ColorTranslator.FromHtml("#777777");
        public override System.Drawing.Color TitleFontColor => ColorTranslator.FromHtml("#777777");
        public override System.Drawing.Color TickLabelColor => ColorTranslator.FromHtml("#AAAAAA");
        public override System.Drawing.Color TickMajorColor => ColorTranslator.FromHtml("#eaeaf2");
        public override System.Drawing.Color TickMinorColor => ColorTranslator.FromHtml("#eaeaf2");
    }
}
