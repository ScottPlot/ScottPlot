using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Renderer
{
    public class Point
    {
        public float X;
        public float Y;
        public override string ToString() => $"({X}, {Y})";

        public Point() { }
        public Point(float x, float y) => (X, Y) = (x, y);

        public Point Shift(float dX, float dY) => new Point(X + dX, Y + dY);
        public Point Round() => new Point((int)X, (int)Y);
    }
}
