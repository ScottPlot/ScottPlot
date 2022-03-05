using System.Drawing;

namespace ScottPlot.Styles
{
    internal class Light1 : Default
    {
        public override Color FrameColor => ColorTranslator.FromHtml("#7f7f7f");
        public override Color TickLabelColor => ColorTranslator.FromHtml("#7f7f7f");
        public override Color TickMajorColor => ColorTranslator.FromHtml("#7f7f7f");
        public override Color TickMinorColor => ColorTranslator.FromHtml("#7f7f7f");
    }
}
