using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Config
{
    public class Layout
    {
        // settings here relate to positioning of objects on the figure

        public int padOnAllSides = 5; // useful for intentionally adding spacing
        public int[] paddingBySide = new int[] { 5, 5, 5, 5 }; // X1, X2, Y1, Y2

        public bool displayAxisFrames = true;
        public bool[] displayFrameByAxis = new bool[] { true, true, true, true };

        public bool tighteningOccurred = false;

        private Graphics gfx = Graphics.FromHwnd(IntPtr.Zero);

        public void Tighten(Ticks ticks, TextLabel title, TextLabel xLabel, TextLabel yLabel)
        {

            // "tighten" the plot by reducing whitespce between labels, data, and the edge of the figure
            if (ticks.x == null)
                return;

            int tickLetterHeight = (int)gfx.MeasureString("test", ticks.font).Height;

            // top
            paddingBySide[3] = 1;
            paddingBySide[3] += Math.Max((int)title.height, tickLetterHeight);
            paddingBySide[3] += padOnAllSides;

            // bottom
            int xLabelHeight = (int)gfx.MeasureString(xLabel.text, xLabel.font).Height;
            paddingBySide[2] = Math.Max(xLabelHeight, tickLetterHeight);
            paddingBySide[2] += tickLetterHeight;
            paddingBySide[2] += padOnAllSides;

            // left
            SizeF yLabelSize = gfx.MeasureString(yLabel.text, yLabel.font);
            paddingBySide[0] = (int)yLabelSize.Height;
            paddingBySide[0] += (int)ticks.y.maxLabelSize.Width;
            paddingBySide[0] += padOnAllSides;

            // right
            paddingBySide[1] = (int)ticks.y.maxLabelSize.Width / 2;
            paddingBySide[1] += padOnAllSides;

            // use no padding on sides with sides with disabled frames
            for (int i = 0; i < 4; i++)
            {
                if (!displayFrameByAxis[i])
                    paddingBySide[i] = 0;
            }

            // override for frameles
            if (!displayAxisFrames)
                paddingBySide = new int[] { 0, 0, 0, 0 };

            tighteningOccurred = true;
        }
    }
}
