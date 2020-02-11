using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Statistics
{
    public struct Box
    {
        public bool outline;
        public bool fill;
        public double min;
        public double max;
        public double width;
        public System.Drawing.Color lineColor;
        public float lineWidth;
        public System.Drawing.Color fillColor;
    }

    public struct Whisker
    {
        public bool visible;
        public double min;
        public double max;
        public double width;
        public System.Drawing.Color lineColor;
        public float lineWidth;
    }

    public struct Midline
    {
        public bool visible;
        public double position;
        public double width;
        public System.Drawing.Color lineColor;
        public float lineWidth;
        public MarkerShape markerShape;
        public float markerSize;
    }

    public class BoxAndWhisker
    {
        public Box box = new Box();
        public Whisker whisker = new Whisker();
        public Midline midline = new Midline();
        public double xPosition;
        public string label;
        public double[] points;

        public BoxAndWhisker(double xPosition)
        {
            this.xPosition = xPosition;
            ResetStyle();
        }

        private void ResetStyle()
        {
            box.outline = true;
            box.fill = true;
            box.lineColor = System.Drawing.Color.Black;
            box.lineWidth = 2;
            box.fillColor = System.Drawing.Color.White;
            box.width = .4;

            whisker.visible = true;
            whisker.lineColor = System.Drawing.Color.Black;
            whisker.lineWidth = 2;
            whisker.width = .4;

            midline.visible = true;
            midline.lineColor = System.Drawing.Color.Black;
            midline.lineWidth = 2;
            midline.markerShape = MarkerShape.filledCircle;
            midline.markerSize = 3;
            midline.width = .4;
        }
    }
}
