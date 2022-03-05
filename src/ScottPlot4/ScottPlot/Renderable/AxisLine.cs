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
        public float PixelOffset;

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            if (IsVisible == false)
                return;

            using (var gfx = GDI.Graphics(bmp, dims, lowQuality, false))
            using (var pen = GDI.Pen(Color, Width))
            {
                float left = dims.DataOffsetX;
                float right = dims.DataOffsetX + dims.DataWidth;
                float top = dims.DataOffsetY;
                float bottom = dims.DataOffsetY + dims.DataHeight;

                if (Edge == Edge.Bottom)
                    gfx.DrawLine(pen, left, bottom + PixelOffset, right, bottom + PixelOffset);
                else if (Edge == Edge.Left)
                    gfx.DrawLine(pen, left - PixelOffset, bottom, left - PixelOffset, top);
                else if (Edge == Edge.Right)
                    gfx.DrawLine(pen, right + PixelOffset, bottom, right + PixelOffset, top);
                else if (Edge == Edge.Top)
                    gfx.DrawLine(pen, left, top - PixelOffset, right, top - PixelOffset);
                else
                    throw new NotImplementedException();
            }
        }
    }
}
