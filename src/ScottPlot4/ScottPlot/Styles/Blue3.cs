using System.Drawing;

namespace ScottPlot.Styles
{
    internal class Blue3 : Default
    {
        public override Color FigureBackgroundColor => ColorTranslator.FromHtml("#001021");
        public override Color DataBackgroundColor => ColorTranslator.FromHtml("#021d38");
        public override Color FrameColor => ColorTranslator.FromHtml("#d3d3d3");
        public override Color GridLineColor => ColorTranslator.FromHtml("#273c51");
        public override Color AxisLabelColor => ColorTranslator.FromHtml("#d3d3d3");
        public override Color TitleFontColor => ColorTranslator.FromHtml("#FFFFFF");
        public override Color TickLabelColor => ColorTranslator.FromHtml("#d3d3d3");
        public override Color TickMajorColor => ColorTranslator.FromHtml("#d3d3d3");
        public override Color TickMinorColor => ColorTranslator.FromHtml("#d3d3d3");
    }
}
