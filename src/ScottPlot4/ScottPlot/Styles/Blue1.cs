using System.Drawing;

namespace ScottPlot.Styles
{
    public class Blue1 : Default
    {
        public override Color FigureBackgroundColor => ColorTranslator.FromHtml("#07263b");
        public override Color DataBackgroundColor => ColorTranslator.FromHtml("#0b3049");
        public override Color FrameColor => ColorTranslator.FromHtml("#145665");
        public override Color GridLineColor => ColorTranslator.FromHtml("#0e3d54");
        public override Color AxisLabelColor => ColorTranslator.FromHtml("#b5bec5");
        public override Color TitleFontColor => ColorTranslator.FromHtml("#d0dae2");
        public override Color TickLabelColor => ColorTranslator.FromHtml("#b5bec5");
        public override Color TickMajorColor => ColorTranslator.FromHtml("#145665");
        public override Color TickMinorColor => ColorTranslator.FromHtml("#145665");
    }
}
