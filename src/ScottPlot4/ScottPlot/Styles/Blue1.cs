using System.Drawing;

namespace ScottPlot.Styles
{
    public class Blue1 : Default
    {
        public override System.Drawing.Color FigureBackgroundColor => ColorTranslator.FromHtml("#07263b");
        public override System.Drawing.Color DataBackgroundColor => ColorTranslator.FromHtml("#0b3049");
        public override System.Drawing.Color FrameColor => ColorTranslator.FromHtml("#145665");
        public override System.Drawing.Color GridLineColor => ColorTranslator.FromHtml("#0e3d54");
        public override System.Drawing.Color AxisLabelColor => ColorTranslator.FromHtml("#b5bec5");
        public override System.Drawing.Color TitleFontColor => ColorTranslator.FromHtml("#d0dae2");
        public override System.Drawing.Color TickLabelColor => ColorTranslator.FromHtml("#b5bec5");
        public override System.Drawing.Color TickMajorColor => ColorTranslator.FromHtml("#145665");
        public override System.Drawing.Color TickMinorColor => ColorTranslator.FromHtml("#145665");
    }
}
