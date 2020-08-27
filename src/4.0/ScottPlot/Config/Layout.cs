using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Config
{
    public class Layout
    {
        // users can configure these
        public int yLabelWidth = 20;
        public int yScaleWidth = 40;
        public int y2LabelWidth = 20;
        public int y2ScaleWidth = 40;
        public int titleHeight = 20;
        public int xLabelHeight = 20;
        public int xScaleHeight = 20;

        public bool[] displayFrameByAxis; // TODO: MOVE THIS TO ANOTHER CLASS
        public bool displayAxisFrames = true; // TODO: MOVE THIS TO ANOTHER CLASS

        public bool tighteningOccurred = false;

        public SHRect plot { get; private set; }
        public SHRect data { get; private set; }

        public SHRect yLabel { get; private set; }
        public SHRect yScale { get; private set; }
        public SHRect y2Label { get; private set; }
        public SHRect y2Scale { get; private set; }
        public SHRect title { get; private set; }
        public SHRect xLabel { get; private set; }
        public SHRect xScale { get; private set; }

        public Layout()
        {
            displayFrameByAxis = new bool[] { true, true, true, true };
            Update(640, 480);
        }

        public Layout(int width, int height)
        {
            Update(width, height);
        }

        public void Update(int width, int height)
        {
            plot = new SHRect(0, width, height, 0);

            title = new SHRect(plot);
            title.ShrinkTo(top: titleHeight);

            yLabel = new SHRect(plot);
            yLabel.ShrinkTo(left: yLabelWidth);

            yScale = new SHRect(plot);
            yScale.ShrinkTo(left: yScaleWidth);
            yScale.Shift(rightward: yLabel.Width);

            y2Label = new SHRect(plot);
            y2Label.ShrinkTo(right: y2LabelWidth);

            y2Scale = new SHRect(plot);
            y2Scale.ShrinkTo(right: y2ScaleWidth);
            y2Scale.Shift(rightward: -y2Label.Width);

            xLabel = new SHRect(plot);
            xLabel.ShrinkTo(bottom: xLabelHeight);

            xScale = new SHRect(plot);
            xScale.ShrinkTo(bottom: xScaleHeight);
            xScale.Shift(downward: -xLabel.Height);

            // the data rectangle is what is left over
            data = new SHRect(plot);
            data.ShrinkBy(top: title.Height);
            data.ShrinkBy(left: yLabel.Width + yScale.Width);
            data.ShrinkBy(right: y2Label.Width + y2Scale.Width);
            data.ShrinkBy(bottom: xLabel.Height + xScale.Height);

            // shrink labels and scales to match dataRect
            yLabel.MatchVert(data);
            yScale.MatchVert(data);
            y2Label.MatchVert(data);
            y2Scale.MatchVert(data);
            xLabel.MatchHoriz(data);
            xScale.MatchHoriz(data);

            // shrink the title to align it with the data area
            title.MatchHoriz(data);
        }
    }
}
