using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Renderer
{
    class Rectangle
    {
        public Point Point;
        public Size Size;

        public Rectangle()
        {
            Point = new Point();
            Size = new Size();
        }

        public Rectangle(Point point, Size size)
        {
            Point = point;
            Size = size;
        }

        public Rectangle(float x, float y, float width, float height)
        {
            Point = new Point(x, y);
            Size = new Size(width, height);
        }
    }
}
