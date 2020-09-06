using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Renderer
{
    public class Size
    {
        public float Width;
        public float Height;
        public override string ToString() => $"[{Width}, {Height}]";

        public Size() { }
        public Size(float width, float height) => (Width, Height) = (width, height);

        public Size Expand(float width, float height) => new Size(Math.Max(Width, width), Math.Max(Height, height));
        public Size Expand(Size size) => Expand(size.Width, size.Height);
        public Size Round() => new Size((int)Width, (int)Height);
    }
}
