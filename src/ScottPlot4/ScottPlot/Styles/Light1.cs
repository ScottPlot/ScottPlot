using System.Drawing;

namespace ScottPlot.Styles
{
    internal class Light1 : Default
    {
        public override System.Drawing.Color FrameColor => ColorTranslator.FromHtml("#7f7f7f");
        public override System.Drawing.Color TickLabelColor => ColorTranslator.FromHtml("#7f7f7f");
        public override System.Drawing.Color TickMajorColor => ColorTranslator.FromHtml("#7f7f7f");
        public override System.Drawing.Color TickMinorColor => ColorTranslator.FromHtml("#7f7f7f");
    }
}
