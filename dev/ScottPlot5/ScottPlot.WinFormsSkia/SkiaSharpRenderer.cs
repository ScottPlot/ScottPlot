using ScottPlot.Renderer;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.WinFormsSkia
{
    class SkiaSharpRenderer : IRenderer
    {
        public float Width { get; private set; }
        public float Height { get; private set; }

        public SKColor Convert(Color c) => new SKColor(c.R, c.G, c.B, c.A);
        public SKPoint Convert(Point pt) => new SKPoint(pt.X, pt.Y);
        public SKRect Convert(Point pt, Size sz) => new SKRect(pt.X, pt.Y, pt.X + sz.Width, pt.Y + sz.Height);
        
        private bool IsAntiAlias = true;

        private readonly SKCanvas Canvas;
        public SkiaSharpRenderer(SKCanvas canvas, float width, float height)
        {
            Canvas = canvas;
            Width = width;
            Height = height;
        }

        public void AntiAlias(bool antiAlias)
        {
            IsAntiAlias = antiAlias;
        }

        public void Clear(Color color)
        {
            Canvas.Clear(Convert(color));
        }

        public void Clip(Point point, Size size)
        {
            Canvas.Save();
            Canvas.ClipRect(Convert(point, size));
        }

        public void ClipReset()
        {
            Canvas.Restore();
        }

        public void Dispose()
        {
            // TODO
        }

        public void DrawLine(Point pt1, Point pt2, Color color, float width, bool rounded = true)
        {
            DrawLines(new Point[] { pt1, pt2 }, color, width, rounded);
        }

        public void DrawLines(Point[] points, Color color, float width, bool rounded = true)
        {
            var paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = width,
                Color = Convert(color),
                StrokeCap = rounded ? SKStrokeCap.Round : SKStrokeCap.Butt,
                StrokeJoin = rounded ? SKStrokeJoin.Round : SKStrokeJoin.Miter,
                IsAntialias = IsAntiAlias,
            };

            var path = new SKPath();
            path.MoveTo(points[0].X, points[0].Y);
            for (int i = 1; i < points.Length; i++)
                path.LineTo(points[i].X, points[i].Y);

            Canvas.DrawPath(path, paint);
        }

        public void DrawRectangle(Point point, Size size, Color color, float width)
        {
            var paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = width,
                Color = Convert(color),
            };
            Canvas.DrawRect(Convert(point, size), paint);
        }

        public void DrawText(Point point, string text, Color color, Font font)
        {
            if (text is null)
                return;

            SKTextAlign ta = new SKTextAlign();
            if (font.HorizontalAlignment == Renderable.HorizontalAlignment.Left)
                ta = SKTextAlign.Left;
            else if (font.HorizontalAlignment == Renderable.HorizontalAlignment.Center)
                ta = SKTextAlign.Center;
            else if (font.HorizontalAlignment == Renderable.HorizontalAlignment.Right)
                ta = SKTextAlign.Right;

            var paint = new SKPaint
            {
                Color = Convert(color),
                Typeface = SKTypeface.FromFamilyName(font.Name),
                TextSize = font.Size,
                TextAlign = ta,
                IsAntialias = true
            };

            var bounds = new SKRect();
            paint.MeasureText(text, ref bounds);

            if (font.VerticalAlignment == Renderable.VerticalAlignment.Top)
                point.Y += bounds.Height;
            else if (font.VerticalAlignment == Renderable.VerticalAlignment.Center)
                point.Y += bounds.Height / 2;

            Canvas.DrawText(text, Convert(point), paint);
        }

        public void FillRectangle(Point point, Size size, Color color)
        {
            var paint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = Convert(color),
            };
            Canvas.DrawRect(Convert(point, size), paint);
        }

        public Size MeasureText(string text, Font font)
        {
            if (text is null)
                return new Size(0, 0);

            var paint = new SKPaint
            {
                Typeface = SKTypeface.FromFamilyName(font.Name),
                TextSize = font.Size
            };

            var bounds = new SKRect();
            paint.MeasureText(text, ref bounds);
            return new Size(bounds.Width, bounds.Height);
        }

        public void Rotate(float angle, Point center)
        {
            Canvas.Save();
            Canvas.Translate(Convert(center));
            Canvas.RotateDegrees(angle);
        }

        public void RotateReset()
        {
            Canvas.Restore();
        }
    }
}
