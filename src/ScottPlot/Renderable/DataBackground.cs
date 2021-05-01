using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Renderable
{
    public class DataBackground : IRenderable
    {
        public Color Color { get; set; } = Color.White;
        public bool IsVisible { get; set; } = true;

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            using (var gfx = GDI.Graphics(bmp, dims, lowQuality: true, false))
            using (var brush = GDI.Brush(Color))
            {
                var dataRect = new RectangleF(x: dims.DataOffsetX, y: dims.DataOffsetY, width: dims.DataWidth, height: dims.DataHeight);
                gfx.FillRectangle(brush, dataRect);
            }
        }
    }
}
