using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Styles
{
    internal class Earth : Default
    {

        public override Color FigureBackgroundColor => Color.SandyBrown;
        public override Color DataBackgroundColor => Color.SaddleBrown;
        public override Color GridLineColor => Color.Sienna;
        public override Color FrameColor => Color.Brown;
        public override Color TitleFontColor => Color.Brown;
        public override Color AxisLabelColor => Color.Brown;
        public override Color TickLabelColor => Color.Brown;
        public override Color TickMajorColor => Color.Brown;
        public override Color TickMinorColor => Color.Brown;


    }
}
