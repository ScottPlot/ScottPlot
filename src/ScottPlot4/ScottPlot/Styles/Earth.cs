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

        public override System.Drawing.Color FigureBackgroundColor => System.Drawing.Color.SandyBrown;
        public override System.Drawing.Color DataBackgroundColor => System.Drawing.Color.SaddleBrown;
        public override System.Drawing.Color GridLineColor => System.Drawing.Color.Sienna;
        public override System.Drawing.Color FrameColor => System.Drawing.Color.Brown;
        public override System.Drawing.Color TitleFontColor => System.Drawing.Color.Brown;
        public override System.Drawing.Color AxisLabelColor => System.Drawing.Color.Brown;
        public override System.Drawing.Color TickLabelColor => System.Drawing.Color.Brown;
        public override System.Drawing.Color TickMajorColor => System.Drawing.Color.Brown;
        public override System.Drawing.Color TickMinorColor => System.Drawing.Color.Brown;


    }
}
