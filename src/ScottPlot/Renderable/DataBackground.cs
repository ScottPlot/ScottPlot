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

        public void Render(PlotDimensions dims, Graphics gfx)
        {
            gfx.ClipToDataArea(dims, false, true);
            using (var brush = GDI.Brush(Color))
            {
                var dataRect = new RectangleF(x: dims.DataOffsetX, y: dims.DataOffsetY, width: dims.DataWidth, height: dims.DataHeight);
                gfx.FillRectangle(brush, dataRect);
            }
        }
    }
}
