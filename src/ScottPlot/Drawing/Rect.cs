using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Drawing
{
    public class Rect
    {
        public double left, right, bottom, top;

        public Point upperLeft { get { return new Point(left, bottom); } }
        public Point upperRight { get { return new Point(right, bottom); } }
        public Point lowerLeft { get { return new Point(left, top); } }
        public Point lowerRight { get { return new Point(right, top); } }

        public Size size { get { return new Size(right - left, top - bottom); } }

        public Rect(double left, double right, double bottom, double top)
        {
            this.left = left;
            this.right = right;
            this.bottom = bottom;
            this.top = top;
        }

        public Rect(Size size, Point topLeft)
        {
            left = topLeft.x;
            right = topLeft.x + size.width;

            bottom = topLeft.y;
            top = topLeft.y + size.height;
        }
    }
}
