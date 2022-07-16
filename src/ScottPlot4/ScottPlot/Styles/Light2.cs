using System.Drawing;

namespace ScottPlot.Styles
{
    internal class Light2 : Default
    {
        public override System.Drawing.Color FigureBackgroundColor => ColorTranslator.FromHtml("#e4e6ec");
        public override System.Drawing.Color DataBackgroundColor => ColorTranslator.FromHtml("#f1f3f7");
        public override System.Drawing.Color FrameColor => ColorTranslator.FromHtml("#145665");
        public override System.Drawing.Color GridLineColor => ColorTranslator.FromHtml("#e5e7ea");
        public override System.Drawing.Color TickLabelColor => ColorTranslator.FromHtml("#77787b");
        public override System.Drawing.Color TickMajorColor => ColorTranslator.FromHtml("#77787b");
        public override System.Drawing.Color TickMinorColor => ColorTranslator.FromHtml("#77787b");
    }
}
