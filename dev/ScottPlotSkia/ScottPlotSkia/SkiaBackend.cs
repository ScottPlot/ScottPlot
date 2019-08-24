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
    public class SkiaBackend : IGraphicBackend, IDisposable
    {
        GRContext context = null;
        SKSurface surface = null;
        SKCanvas canvas = null;
        bool AA = true;
        public SkiaBackend(int width, int height)
        {
            var info = new SKImageInfo(width, height);

            try
            {
                var glInterface = GRGlInterface.CreateNativeAngleInterface();
                context = GRContext.Create(GRBackend.OpenGL, glInterface);
            }
            catch
            {
                context = GRContext.Create(GRBackend.OpenGL);
            }

            surface?.Dispose();
            surface = SKSurface.Create(context, true, info);
            canvas = surface.Canvas;
        }
        public void Clear(Color color)
        {
            canvas.Clear(color.ToSKColor());
        }

        public void Dispose()
        {
            canvas?.Dispose();
            surface?.Dispose();
            context?.Dispose();
        }

        public void DrawEllipse(Pen pen, RectangleF rect)
        {
            throw new NotImplementedException();
        }

        // uses for drawing grid lines so only pen.Color can be used
        public void DrawLine(Pen pen, int x1, int y1, int x2, int y2)
        {
            using (var paint = new SKPaint() { Color = pen.Color.ToSKColor(), IsAntialias = AA })
                canvas.DrawLine((float)x1, (float)y1, (float)x2, (float)y2, paint);
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
            var paint = new SKPaint()
            {
                Color = pen.Color.ToSKColor(),
                IsAntialias = AA,

            };
            canvas.DrawPoints(SKPointMode.Polygon, linePoints.Select(x => x.ToSKPoint()).ToArray(), paint);
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
            throw new NotImplementedException();
        }

        public void DrawString(string text, Font font, Brush brush, PointF point)
        {
            throw new NotImplementedException();
        }

        public void FillEllipse(Brush brush, float x, float y, float widht, float height)
        {
            SKPaint paint = new SKPaint() { Color = (brush as SolidBrush).Color.ToSKColor(), IsAntialias = AA };
            canvas.DrawOval(SKRect.Create(x, y, widht, height), paint);
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
            throw new NotImplementedException();
        }

        public void FillRectangle(Brush brush, float x, float y, float widht, float height)
        {
            throw new NotImplementedException();
        }

        public Bitmap GetBitmap()
        {
            var snapshot = surface.Snapshot();
            var bitmap = snapshot.ToBitmap();
            snapshot.Dispose();
            return bitmap; // must be disposed            
        }

        public SizeF MeasureString(string text, Font font)
        {
            throw new NotImplementedException();
        }

        public void Resize(int width, int height)
        {
            if (width > 0 && height > 0)
            {
                var info = new SKImageInfo(width, height);
                canvas?.Dispose();
                surface?.Dispose();
                surface = SKSurface.Create(context, true, info);
                canvas = surface.Canvas;
            }
        }

        public void SetAntiAlias(bool enabled)
        {
            AA = enabled;
        }
    }
}
