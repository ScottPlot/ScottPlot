using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Renderable
{
    public class AxisLine : IRenderable
    {
        public bool IsVisible { get; set; } = true;
        public Color Color = Color.Black;
        public float Width = 1;
        public Edge Edge;

        public void Render(PlotDimensions2D dims, Bitmap bmp, bool lowQuality = false)
        {
            if (IsVisible == false)
                return;

            using (var gfx = GDI.Graphics(bmp, lowQuality))
            using (var pen = GDI.Pen(Color, Width))
            {
                PointF bottomLeft = new PointF(dims.DataOffsetX, dims.DataOffsetY + dims.DataHeight);
                PointF topLeft = new PointF(dims.DataOffsetX, dims.DataOffsetY);
                PointF topRight = new PointF(dims.DataOffsetX + dims.DataWidth, dims.DataOffsetY);
                PointF bottomRight = new PointF(dims.DataOffsetX + dims.DataWidth, dims.DataOffsetY + dims.DataHeight);

                if (Edge == Edge.Bottom)
                    gfx.DrawLine(pen, bottomLeft, bottomRight);
                else if (Edge == Edge.Left)
                    gfx.DrawLine(pen, bottomLeft, topLeft);
                else if (Edge == Edge.Right)
                    gfx.DrawLine(pen, bottomRight, topRight);
                else if (Edge == Edge.Top)
                    gfx.DrawLine(pen, topLeft, topRight);
                else
                    throw new NotImplementedException();
            }
        }
    }
}
