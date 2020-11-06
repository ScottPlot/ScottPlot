using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Renderable
{
    public class AxisTitle : IRenderable
    {
        public bool IsVisible { get; set; }
        public string Label = null;
        public Drawing.Font Font = new Drawing.Font() { Size = 16 };
        public Edge Edge;

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
                    gfx.DrawString(Label, font, brush, dataCenterX, dims.Height, sf);
                }
                else if (Edge == Edge.Top)
                {
                    sf.LineAlignment = StringAlignment.Near;
                    gfx.DrawString(Label, font, brush, dataCenterX, 0, sf);
                }
                else if (Edge == Edge.Left)
                {
                    sf.LineAlignment = StringAlignment.Near;
                    gfx.TranslateTransform(0, dataCenterY);
                    gfx.RotateTransform(-90);
                    gfx.DrawString(Label, font, brush, 0, 0, sf);
                    gfx.ResetTransform();
                }
                else if (Edge == Edge.Right)
                {
                    sf.LineAlignment = StringAlignment.Near;
                    gfx.TranslateTransform(dims.Width, dataCenterY);
                    gfx.RotateTransform(90);
                    gfx.DrawString(Label, font, brush, 0, 0, sf);
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
