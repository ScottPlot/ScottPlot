using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace ScottPlot.Renderer
{
    public class SystemDrawingRenderer : IRenderer
    {
        private readonly bool DisposeBitmap;
        private readonly Bitmap Bmp;
        private readonly Graphics Gfx;

        public float Width { get; private set; }
        public float Height { get; private set; }

        private readonly Pen Pen;

        public SystemDrawingRenderer(Bitmap bmp, bool disposeBitmap = false)
        {
            Bmp = bmp;
            DisposeBitmap = disposeBitmap;
            Width = bmp.Width;
            Height = bmp.Height;

            Gfx = Graphics.FromImage(bmp);
            Pen = new Pen(System.Drawing.Color.Black);
        }

        public void Dispose()
        {
            Gfx.Dispose();
            Pen.Dispose();

            if (DisposeBitmap)
                Bmp.Dispose();
        }

        public void AntiAlias(bool antiAlias)
        {
            Gfx.SmoothingMode = antiAlias ?
                System.Drawing.Drawing2D.SmoothingMode.AntiAlias :
                System.Drawing.Drawing2D.SmoothingMode.HighSpeed;

            Gfx.TextRenderingHint = antiAlias ?
                System.Drawing.Text.TextRenderingHint.AntiAlias :
                System.Drawing.Text.TextRenderingHint.SingleBitPerPixel;
        }

        public void Clip(Point point, Size size)
        {
            RectangleF rect = new RectangleF(point.X, point.Y, size.Width, size.Height);
            Gfx.SetClip(rect);
        }

        public void ClipReset()
        {
            Gfx.ResetClip();
        }

        public void Clear(Color color)
        {
            Gfx.Clear(System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B));
        }

        public Size MeasureText(string text, string fontName, float fontSize)
        {
            using (var fnt = new Font(fontName, fontSize))
            {
                var sz = Gfx.MeasureString(text, fnt);
                return new Size(sz.Width, sz.Height);
            }
        }

        public void DrawText(Point point, string text, Color color, string fontName, float fontSize)
        {
            using (var fnt = new Font(fontName, fontSize))
            using (var brush = new SolidBrush(System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B)))
            {
                Gfx.DrawString(text, fnt, brush, point.X, point.Y);
            }
        }

        public void FillRectangle(Point point, Size size, Color color)
        {
            using (var brush = new SolidBrush(System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B)))
            {
                Gfx.FillRectangle(brush, point.X, point.Y, size.Width, size.Height);
            }
        }

        public void DrawRectangle(Point point, Size size, Color color, float width)
        {
            Pen.Color = System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
            Pen.Width = width;
            Gfx.DrawRectangle(Pen, point.X, point.Y, size.Width, size.Height);
        }

        public void DrawLines(PointF[] points, Color color, float width)
        {
            Pen.Color = System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
            Pen.Width = width;
            Gfx.DrawLines(Pen, points);
        }
    }
}
