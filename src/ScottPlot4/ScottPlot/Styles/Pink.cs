using System.Drawing;

namespace ScottPlot.Styles
{
    internal class Pink : Default
    {
        public override System.Drawing.Color FigureBackgroundColor => ColorTranslator.FromHtml("#d7c0d0");
        public override System.Drawing.Color DataBackgroundColor => ColorTranslator.FromHtml("#f7c7db");
        public override System.Drawing.Color FrameColor => ColorTranslator.FromHtml("#f79ad3");
        public override System.Drawing.Color GridLineColor => ColorTranslator.FromHtml("#c86fc9");
        public override System.Drawing.Color TickLabelColor => ColorTranslator.FromHtml("#8e518d");
        public override System.Drawing.Color TickMajorColor => ColorTranslator.FromHtml("#d7c0d0");
        public override System.Drawing.Color TickMinorColor => ColorTranslator.FromHtml("#f79ad3");
    }
}
