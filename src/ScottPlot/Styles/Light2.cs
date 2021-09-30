using System.Drawing;

namespace ScottPlot.Styles
{
    internal class Light2 : Default
    {
        public override Color FigureBackgroundColor => ColorTranslator.FromHtml("#e4e6ec");
        public override Color DataBackgroundColor => ColorTranslator.FromHtml("#f1f3f7");
        public override Color FrameColor => ColorTranslator.FromHtml("#145665");
        public override Color GridLineColor => ColorTranslator.FromHtml("#e5e7ea");
        public override Color TickLabelColor => ColorTranslator.FromHtml("#77787b");
        public override Color TickMajorColor => ColorTranslator.FromHtml("#77787b");
        public override Color TickMinorColor => ColorTranslator.FromHtml("#77787b");
    }
}
