using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Renderable
{
    public class AxisLabel : IRenderable
    {
        public bool IsVisible { get; set; } = true;
        public string Label = null;
        public Drawing.Font Font = new Drawing.Font() { Size = 16 };
        public Edge Edge;
        public float PixelSizePadding;

        public float PixelOffset;
        public float PixelSize;

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            if (IsVisible == false || string.IsNullOrWhiteSpace(Label))
                return;

            float dataCenterX = dims.DataOffsetX + dims.DataWidth / 2;
            float dataCenterY = dims.DataOffsetY + dims.DataHeight / 2;

            using (var gfx = GDI.Graphics(bmp, lowQuality))
            using (var font = GDI.Font(Font))
            using (var brush = GDI.Brush(Font.Color))
            using (var sf = GDI.StringFormat(HorizontalAlignment.Center, VerticalAlignment.Lower))
            {
                if (Edge == Edge.Bottom)
                {
                    sf.LineAlignment = StringAlignment.Far;
                    float y = dims.DataOffsetY + dims.DataHeight + PixelOffset + PixelSize;
                    gfx.TranslateTransform(dataCenterX, y);
                    gfx.DrawString(Label, font, brush, 0, -PixelSizePadding, sf);
                    gfx.ResetTransform();
                }
                else if (Edge == Edge.Top)
                {
                    sf.LineAlignment = StringAlignment.Near;
                    float y = dims.DataOffsetY - PixelOffset - PixelSize;
                    gfx.TranslateTransform(dataCenterX, y);
                    gfx.DrawString(Label, font, brush, 0, PixelSizePadding, sf);
                    gfx.ResetTransform();
                }
                else if (Edge == Edge.Left)
                {
                    sf.LineAlignment = StringAlignment.Near;
                    float x = dims.DataOffsetX - PixelOffset - PixelSize;
                    gfx.TranslateTransform(x, dataCenterY);
                    gfx.RotateTransform(-90);
                    gfx.DrawString(Label, font, brush, 0, PixelSizePadding, sf);
                    gfx.ResetTransform();
                }
                else if (Edge == Edge.Right)
                {
                    sf.LineAlignment = StringAlignment.Near;
                    float x = dims.DataOffsetX + dims.DataWidth + PixelOffset + PixelSize;
                    gfx.TranslateTransform(x, dataCenterY);
                    gfx.RotateTransform(90);
                    gfx.DrawString(Label, font, brush, 0, PixelSizePadding, sf);
                    gfx.ResetTransform();
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}
