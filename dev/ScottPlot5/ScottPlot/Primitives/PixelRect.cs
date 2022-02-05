using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Primitives
{
    internal class PixelRect
    {
        public float Bottom;
        public float Top;
        public float Right;
        public float Left;

        public float Width;
        public float Height;

        public Pixel TopLeft => new(Left, Top);
        public Pixel TopRight => new(Right, Top);
        public Pixel BottomLeft => new(Left, Bottom);
        public Pixel BottomRight => new(Right, Bottom);

        public float Area => (float)Math.Max(0, Width) * (float)Math.Max(0, Height);
        public Microsoft.Maui.Graphics.PointF LocationF => new(Left, Top);
        public Microsoft.Maui.Graphics.SizeF SizeF => new(Width, Height);
        public Microsoft.Maui.Graphics.RectangleF RectF => new(LocationF, SizeF);
    }
}
