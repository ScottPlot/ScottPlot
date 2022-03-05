using System.Drawing;

namespace ScottPlot.Styles
{
    internal class Pink : Default
    {
        public override Color FigureBackgroundColor => ColorTranslator.FromHtml("#d7c0d0");
        public override Color DataBackgroundColor => ColorTranslator.FromHtml("#f7c7db");
        public override Color FrameColor => ColorTranslator.FromHtml("#f79ad3");
        public override Color GridLineColor => ColorTranslator.FromHtml("#c86fc9");
        public override Color TickLabelColor => ColorTranslator.FromHtml("#8e518d");
        public override Color TickMajorColor => ColorTranslator.FromHtml("#d7c0d0");
        public override Color TickMinorColor => ColorTranslator.FromHtml("#f79ad3");
    }
}
