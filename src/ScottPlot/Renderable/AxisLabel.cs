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
        public Bitmap ImageLabel = null;
        public Drawing.Font Font = new Drawing.Font() { Size = 16 };
        public Edge Edge;
        public float PixelSizePadding;

        public float PixelOffset;
        public float PixelSize;

        public SizeF Measure()
        {
            if (ImageLabel != null)
            {
                SizeF size = new SizeF(ImageLabel.Width * 1.5f, ImageLabel.Height * 1.5f);
                if (Edge == Edge.Left || Edge == Edge.Right) // Swapping required because Height is the distance orthogonal to the axis, i.e. YAxis labels are treated as if they're rotated 90 degrees.
                {
                    float tmp = size.Width;
                    size.Width = size.Height;
                    size.Height = tmp;
                }

                return size;
            }
            else
            {
                return GDI.MeasureString(Label, Font);
            }
        }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            if (IsVisible == false || (string.IsNullOrWhiteSpace(Label) && ImageLabel == null))
                return;

            float dataCenterX = dims.DataOffsetX + dims.DataWidth / 2;
            float dataCenterY = dims.DataOffsetY + dims.DataHeight / 2;
            float x = dataCenterX;
            float y = dataCenterY;
            int rotation = 0;
            bool subtract = false;

            using (var gfx = GDI.Graphics(bmp, lowQuality))
            using (var font = GDI.Font(Font))
            using (var brush = GDI.Brush(Font.Color))
            using (var sf = GDI.StringFormat(HorizontalAlignment.Center, VerticalAlignment.Lower))
            {
                if (Edge == Edge.Bottom)
                {
                    sf.LineAlignment = StringAlignment.Far;
                    y = dims.DataOffsetY + dims.DataHeight + PixelOffset + PixelSize;
                    subtract = true;
                }
                else if (Edge == Edge.Top)
                {
                    sf.LineAlignment = StringAlignment.Near;
                    y = dims.DataOffsetY - PixelOffset - PixelSize;
                }
                else if (Edge == Edge.Left)
                {
                    sf.LineAlignment = StringAlignment.Near;
                    x = dims.DataOffsetX - PixelOffset - PixelSize;
                    rotation = -90;
                }
                else if (Edge == Edge.Right)
                {
                    sf.LineAlignment = StringAlignment.Near;
                    x = dims.DataOffsetX + dims.DataWidth + PixelOffset + PixelSize;
                    rotation = 90;
                }
                else
                {
                    throw new NotImplementedException();
                }

                float padding = subtract ? -PixelSizePadding : PixelSizePadding;
                gfx.TranslateTransform(x, y);

                if (ImageLabel == null)
                {
                    gfx.RotateTransform(rotation);
                    gfx.DrawString(Label, font, brush, 0, padding, sf);
                }
                else
                {
                    float xOffset = 0;
                    float yOffset = 0;

                    switch (Edge)
                    {
                        case Edge.Top:
                            xOffset = -ImageLabel.Width / 2;
                            yOffset = ImageLabel.Height / 4;
                            break;

                        case Edge.Bottom:
                            xOffset = -ImageLabel.Width / 2;
                            yOffset = -3 * ImageLabel.Height / 2;
                            break;

                        case Edge.Left:
                            xOffset = 0;
                            yOffset = -ImageLabel.Height / 2;
                            break;

                        case Edge.Right:
                            xOffset = -3 * ImageLabel.Width / 2;
                            yOffset = -ImageLabel.Height / 2;
                            break;

                    }

                    gfx.DrawImage(ImageLabel, xOffset, yOffset);
                }

                gfx.ResetTransform();
            }
        }
    }
}
