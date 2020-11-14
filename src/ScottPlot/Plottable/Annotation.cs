using ScottPlot.Ticks;
using ScottPlot.Drawing;
using ScottPlot.Renderable;
using System;
using System.Drawing;

namespace ScottPlot.Plottable
{
    public class Annotation : IRenderable
    {
        public double xPixel;
        public double yPixel;
        public string label;

        public string FontName;
        public Color FontColor = Color.Red;
        public float FontSize = 12;
        public bool FontBold = false;

        public bool Background = true;
        public Color BackgroundColor = Color.White;

        public bool Shadow = true;
        public Color ShadowColor = Color.FromArgb(25, Color.Black);

        public bool Border = true;
        public float BorderWidth = 2;
        public Color BorderColor = Color.Black;

        public bool IsVisible { get; set; } = true;

        public override string ToString() => $"PlottableAnnotation at ({xPixel} px, {yPixel} px)";

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            using (var gfx = GDI.Graphics(bmp, lowQuality))
            using (var font = GDI.Font(FontName, FontSize, FontBold))
            using (var fontBrush = new SolidBrush(FontColor))
            using (var shadowBrush = new SolidBrush(ShadowColor))
            using (var backgroundBrush = new SolidBrush(BackgroundColor))
            using (var borderPen = new Pen(BorderColor, BorderWidth))
            {
                SizeF size = GDI.MeasureString(gfx, label, font);

                double x = (xPixel >= 0) ? xPixel : dims.DataWidth + xPixel - size.Width;
                double y = (yPixel >= 0) ? yPixel : dims.DataHeight + yPixel - size.Height;
                PointF location = new PointF((float)x + dims.DataOffsetX, (float)y + dims.DataOffsetY);

                if (Background && Shadow)
                    gfx.FillRectangle(shadowBrush, location.X + 5, location.Y + 5, size.Width, size.Height);

                if (Background)
                    gfx.FillRectangle(backgroundBrush, location.X, location.Y, size.Width, size.Height);

                if (Border)
                    gfx.DrawRectangle(borderPen, location.X, location.Y, size.Width, size.Height);

                gfx.DrawString(label, font, fontBrush, location);
            }
        }
    }
}
