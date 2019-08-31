using ScottPlot;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotSkia
{
    public class SkiaBackend : IGraphicBackend
    {
        public SKCanvas canvas = null;
        public bool AA = true;
        private Bitmap fakeBitmap = new Bitmap(10, 10);
        public SkiaBackend()
        {
            canvas = new SKCanvas(fakeBitmap.ToSKBitmap());
        }

        public void Clear(Color color)
        {
            canvas.Clear(color.ToSKColor());
        }

        public void FillCircles(Brush brush, PointF[] points, float radius)
        {
            SKPaint paint = new SKPaint() { Color = (brush as SolidBrush).Color.ToSKColor(), IsAntialias = AA, StrokeWidth = radius, StrokeCap = SKStrokeCap.Round };
            canvas.DrawPoints(SKPointMode.Points, points.Select(x => x.ToSKPoint()).ToArray(), paint);
        }

        public void DrawEllipse(Pen pen, RectangleF rect)
        {
            throw new NotImplementedException();
        }

        // uses for drawing grid lines so only pen.Color can be used
        public void DrawLine(Pen pen, int x1, int y1, int x2, int y2)
        {
            canvas.DrawLine((float)x1, (float)y1, (float)x2, (float)y2, pen.ToSKPaint(AA));
        }

        public void DrawLine(Pen pen, PointF start, PointF end)
        {
            throw new NotImplementedException();
        }

        public void DrawLine(Pen pen, float x1, float y1, float x2, float y2)
        {
            throw new NotImplementedException();
        }

        public void DrawLines(Pen pen, PointF[] linePoints)
        {
            canvas.DrawPoints(SKPointMode.Polygon, linePoints.Select(x => new SKPoint(x.X + 0.5f, x.Y + 0.5f)).ToArray(), pen.ToSKPaint(AA));
        }

        public void DrawPolygon(Pen pen, PointF[] curvePoints)
        {
            throw new NotImplementedException();
        }

        public void DrawRectangle(Pen pen, float x, float y, float width, float height)
        {
            throw new NotImplementedException();
        }

        public void DrawRectangle(Pen pen, Rectangle rect)
        {
            SKPaint paint = new SKPaint() { Color = pen.Color.ToSKColor(), IsAntialias = AA, Style = SKPaintStyle.Stroke };
            canvas.DrawRect(rect.ToSKRect(), paint);
        }

        public void DrawString(string text, Font font, Brush brush, PointF point)
        {
            throw new NotImplementedException();
        }

        public void FillEllipse(Brush brush, float x, float y, float widht, float height)
        {
            canvas.DrawOval(SKRect.Create(x, y, widht, height), brush.ToSKPaint());
        }

        public void FillEllipse(Brush brush, RectangleF rect)
        {
            throw new NotImplementedException();
        }

        public void FillPolygon(Brush brush, PointF[] curvePoints)
        {
            throw new NotImplementedException();
        }

        public void FillRectangle(Brush brush, RectangleF rect)
        {
            canvas.DrawRect(new SKRect(rect.Left, rect.Top, rect.Right, rect.Bottom), brush.ToSKPaint(AA));
        }

        public void FillRectangle(Brush brush, float x, float y, float widht, float height)
        {
            throw new NotImplementedException();
        }

        public Bitmap GetBitmap()
        {
            return fakeBitmap;
        }

        public SizeF MeasureString(string text, Font font)
        {
            throw new NotImplementedException();
        }

        public void Resize(int width, int height)
        {
        }

        public void SetAntiAlias(bool enabled)
        {
            AA = enabled;
        }
    }
}
