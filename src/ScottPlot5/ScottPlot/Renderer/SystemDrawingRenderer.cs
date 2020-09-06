using ScottPlot.Renderable;
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

        public void Rotate(float angle, Point center)
        {
            Gfx.TranslateTransform(center.X, center.Y);
            Gfx.RotateTransform(angle);
        }

        public void RotateReset()
        {
            Gfx.ResetTransform();
        }

        public void Clear(Color color)
        {
            Gfx.Clear(System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B));
        }

        public Size MeasureText(string text, Font font)
        {
            using (var fnt = new System.Drawing.Font(font.Name, font.Size))
            {
                var sz = Gfx.MeasureString(text, fnt);
                return new Size(sz.Width, sz.Height);
            }
        }

        public void DrawText(Point point, string text, Color color, Font font)
        {
            using (var sf = new StringFormat())
            using (var fnt = new System.Drawing.Font(font.Name, font.Size))
            using (var brush = new SolidBrush(System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B)))
            {
                if (font.HorizontalAlignment == HorizontalAlignment.Left)
                    sf.Alignment = StringAlignment.Near;
                else if (font.HorizontalAlignment == HorizontalAlignment.Center)
                    sf.Alignment = StringAlignment.Center;
                else if (font.HorizontalAlignment == HorizontalAlignment.Right)
                    sf.Alignment = StringAlignment.Far;

                if (font.VerticalAlignment == VerticalAlignment.Bottom)
                    sf.LineAlignment = StringAlignment.Far;
                else if (font.VerticalAlignment == VerticalAlignment.Center)
                    sf.LineAlignment = StringAlignment.Center;
                else if (font.VerticalAlignment == VerticalAlignment.Top)
                    sf.LineAlignment = StringAlignment.Near;

                Gfx.DrawString(text, fnt, brush, point.X, point.Y, sf);
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

        public void DrawLines(Point[] points, Color color, float width, bool rounded)
        {
            PointF[] sdPoints = new PointF[points.Length];
            for (int i = 0; i < points.Length; i++)
                sdPoints[i] = new PointF(points[i].X, points[i].Y);

            Pen.Color = System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
            Pen.Width = width;
            Pen.StartCap = rounded ? System.Drawing.Drawing2D.LineCap.Round : System.Drawing.Drawing2D.LineCap.Flat;
            Pen.EndCap = rounded ? System.Drawing.Drawing2D.LineCap.Round : System.Drawing.Drawing2D.LineCap.Flat;
            Pen.LineJoin = rounded ? System.Drawing.Drawing2D.LineJoin.Round : System.Drawing.Drawing2D.LineJoin.Miter;

            Gfx.DrawLines(Pen, sdPoints);
        }

        public void DrawLine(Point pt1, Point pt2, Color color, float width, bool rounded)
        {
            Pen.Color = System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
            Pen.Width = width;
            Pen.StartCap = rounded ? System.Drawing.Drawing2D.LineCap.Round : System.Drawing.Drawing2D.LineCap.Flat;
            Pen.EndCap = rounded ? System.Drawing.Drawing2D.LineCap.Round : System.Drawing.Drawing2D.LineCap.Flat;
            Pen.LineJoin = rounded ? System.Drawing.Drawing2D.LineJoin.Round : System.Drawing.Drawing2D.LineJoin.Miter;

            Gfx.DrawLine(Pen, pt1.X, pt1.Y, pt2.X, pt2.Y);
        }
    }
}
