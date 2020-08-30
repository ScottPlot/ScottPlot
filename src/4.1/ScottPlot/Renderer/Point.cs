using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Renderer
{
    public class Point
    {
        public float X;
        public float Y;

        public Point()
        {

        }

        public Point(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Point Shift(float dX, float dY)
        {
            return new Point(X + dX, Y + dY);
        }
    }
}
