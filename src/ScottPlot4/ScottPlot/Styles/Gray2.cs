using System.Drawing;

namespace ScottPlot.Styles
{
    internal class Gray2 : Default
    {
        public override System.Drawing.Color FigureBackgroundColor => ColorTranslator.FromHtml("#131519");
        public override System.Drawing.Color DataBackgroundColor => ColorTranslator.FromHtml("#262626");
        public override System.Drawing.Color FrameColor => ColorTranslator.FromHtml("#757575");
        public override System.Drawing.Color GridLineColor => ColorTranslator.FromHtml("#2d2d2d");
        public override System.Drawing.Color AxisLabelColor => ColorTranslator.FromHtml("#b9b9ba");
        public override System.Drawing.Color TitleFontColor => ColorTranslator.FromHtml("#FFFFFF");
        public override System.Drawing.Color TickLabelColor => ColorTranslator.FromHtml("#757575");
        public override System.Drawing.Color TickMajorColor => ColorTranslator.FromHtml("#757575");
        public override System.Drawing.Color TickMinorColor => ColorTranslator.FromHtml("#757575");
    }
}
