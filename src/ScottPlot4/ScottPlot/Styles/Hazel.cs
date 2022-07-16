using System.Drawing;

namespace ScottPlot.Styles
{
    public class Hazel : Default
    {
        public override System.Drawing.Color FigureBackgroundColor => ColorTranslator.FromHtml("#221a0f");
        public override System.Drawing.Color DataBackgroundColor => ColorTranslator.FromHtml("#362712");
        public override System.Drawing.Color FrameColor => System.Drawing.Color.White;
        public override System.Drawing.Color GridLineColor => ColorTranslator.FromHtml("#221a0f");
        public override System.Drawing.Color AxisLabelColor => System.Drawing.Color.White;
        public override System.Drawing.Color TitleFontColor => System.Drawing.Color.White;
        public override System.Drawing.Color TickLabelColor => System.Drawing.Color.Gray;
        public override System.Drawing.Color TickMajorColor => ColorTranslator.FromHtml("#757575");
        public override System.Drawing.Color TickMinorColor => ColorTranslator.FromHtml("#757575");
    }
}
