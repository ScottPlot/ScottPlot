using System.Drawing;

namespace ScottPlot.Styles
{
    internal class Blue3 : Default
    {
        public override System.Drawing.Color FigureBackgroundColor => ColorTranslator.FromHtml("#001021");
        public override System.Drawing.Color DataBackgroundColor => ColorTranslator.FromHtml("#021d38");
        public override System.Drawing.Color FrameColor => ColorTranslator.FromHtml("#d3d3d3");
        public override System.Drawing.Color GridLineColor => ColorTranslator.FromHtml("#273c51");
        public override System.Drawing.Color AxisLabelColor => ColorTranslator.FromHtml("#d3d3d3");
        public override System.Drawing.Color TitleFontColor => ColorTranslator.FromHtml("#FFFFFF");
        public override System.Drawing.Color TickLabelColor => ColorTranslator.FromHtml("#d3d3d3");
        public override System.Drawing.Color TickMajorColor => ColorTranslator.FromHtml("#d3d3d3");
        public override System.Drawing.Color TickMinorColor => ColorTranslator.FromHtml("#d3d3d3");
    }
}
