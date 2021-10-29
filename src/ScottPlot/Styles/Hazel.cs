using System.Drawing;

namespace ScottPlot.Styles
{
    public class Hazel : Default
    {
        public override Color FigureBackgroundColor => ColorTranslator.FromHtml("#221a0f");
        public override Color DataBackgroundColor => ColorTranslator.FromHtml("#362712");
        public override Color FrameColor => Color.White;
        public override Color GridLineColor => ColorTranslator.FromHtml("#221a0f");
        public override Color AxisLabelColor => Color.White;
        public override Color TitleFontColor => Color.White;
        public override Color TickLabelColor => Color.Gray;
        public override Color TickMajorColor => ColorTranslator.FromHtml("#757575");
        public override Color TickMinorColor => ColorTranslator.FromHtml("#757575");
    }
}
