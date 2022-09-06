using ScottPlot.Drawing;
using ScottPlot.Drawing.Colormaps;
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
        public Bitmap Bitmap { get; set; } = null;

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            if (Bitmap is null)
            {
                RenderSolidColorBackground(dims, bmp);
            }
            else
            {
                RenderImageBackground(dims, bmp);
            }
        }

        private void RenderSolidColorBackground(PlotDimensions dims, Bitmap bmp)
        {
            using var gfx = GDI.Graphics(bmp, dims, lowQuality: true, clipToDataArea: false);
            using var brush = GDI.Brush(Color);

            var dataRect = new RectangleF(
                x: dims.DataOffsetX,
                y: dims.DataOffsetY,
                width: dims.DataWidth,
                height: dims.DataHeight);

            gfx.FillRectangle(brush, dataRect);
        }

        private void RenderImageBackground(PlotDimensions dims, Bitmap bmp)
        {
            using var gfx = GDI.Graphics(bmp, dims, lowQuality: false, clipToDataArea: true);

            float x = dims.DataOffsetX;
            float y = dims.DataOffsetY;
            float width = dims.DataWidth;
            float height = dims.DataHeight;

            // NOTE: increase size by 1 source pixel to prevent anti-aliasing transparency at edges
            x -= width / Bitmap.Width;
            y -= height / Bitmap.Height;
            width += 2 * width / Bitmap.Width;
            height += 2 * height / Bitmap.Height;

            gfx.DrawImage(Bitmap, x, y, width, height);
        }
    }
}
