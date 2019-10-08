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

        public void DrawImage(Image image, Point point)
        {
            // do nothing
        }

        public Size GetSize()
        {
            return fakeBitmap.Size;
        }
        int rotateState;
        public void RotateTransform(float angle)
        {
            rotateState = canvas.Save();
            canvas.RotateDegrees(angle);
        }

        public void ResetRotateTransform()
        {
            canvas.RestoreToCount(rotateState);
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
            canvas.DrawLine(start.ToSKPoint(), end.ToSKPoint(), pen.ToSKPaint());
        }

        public void DrawLine(Pen pen, float x1, float y1, float x2, float y2)
        {
            canvas.DrawLine(new SKPoint(x1, y1), new SKPoint(x2, y2), pen.ToSKPaint());
        }

        public void DrawLines(Pen pen, PointF[] linePoints)
        {
            canvas.DrawPoints(SKPointMode.Polygon, linePoints.Select(x => new SKPoint(x.X + 0.5f, x.Y + 0.5f)).ToArray(), pen.ToSKPaint(AA));
        }

        public void DrawLinesPaired(Pen pen, PointF[] linePoints)
        {
            canvas.DrawPoints(SKPointMode.Lines, linePoints.Select(x => new SKPoint(x.X + 0.5f, x.Y + 0.5f)).ToArray(), pen.ToSKPaint(AA));
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

        public void DrawRectangles(Pen pen, RectangleF[] rects)
        {
            using (var path = new SKPath())
            {
                foreach (var rect in rects)
                    path.AddRect(rect.ToSKRect());
                canvas.DrawPath(path, pen.ToSKPaint());
            }
        }

        public void DrawString(string text, Font font, Brush brush, PointF point, StringFormat format)
        {
            var paint = font.ToSKPaint(brush, true);
            paint.TextAlign = format.Alignment.ToSKTextAlign();
            point.Y += paint.TextSize;
            canvas.DrawText(text, point.X, point.Y, paint);
        }

        public void DrawString(string text, Font font, Brush brush, PointF point)
        {
            var paint = font.ToSKPaint(brush, true);
            point.Y += paint.TextSize;
            canvas.DrawText(text, point.X, point.Y, paint);
        }

        public void FillEllipse(Brush brush, float x, float y, float widht, float height)
        {
            canvas.DrawOval(SKRect.Create(x, y, widht, height), brush.ToSKPaint(AA));
        }

        public void FillEllipse(Brush brush, RectangleF rect)
        {
            canvas.DrawOval(rect.ToSKRect(), brush.ToSKPaint(AA));
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
            canvas.DrawRect(x, y, widht, height, brush.ToSKPaint(AA));
        }

        public void FillRectangles(Brush brush, RectangleF[] rects)
        {
            using (var path = new SKPath())
            {
                foreach (var rect in rects)
                    path.AddRect(rect.ToSKRect());
                canvas.DrawPath(path, brush.ToSKPaint(AA));
            }
        }

        public Bitmap GetBitmap()
        {
            return fakeBitmap;
        }

        public SizeF MeasureString(string text, Font font)
        {
            SKPaint paint = new SKPaint() { TextSize = font.Size * 2 };
            return new SizeF(paint.MeasureText(text), paint.TextSize);
        }

        public void Resize(int width, int height)
        {
        }

        public void SetAntiAlias(bool enabled)
        {
            AA = enabled;
        }

        public void DrawMarkers(PointF[] points, MarkerShape shape, float markerSize, Color color)
        {
            var paint = new SKPaint() { Color = color.ToSKColor(), IsAntialias = AA, StrokeWidth = markerSize / 5 };
            using (var path = new SKPath())
            {
                switch (shape)
                {
                    case MarkerShape.filledCircle:
                        foreach (var p in points)
                        {
                            path.AddCircle(p.X, p.Y, markerSize / 2);
                        }
                        paint.Style = SKPaintStyle.Fill;
                        break;
                    case MarkerShape.openCircle:
                        foreach (var p in points)
                        {
                            path.AddCircle(p.X, p.Y, markerSize / 2);
                        }
                        paint.Style = SKPaintStyle.Stroke;
                        break;
                    case MarkerShape.filledSquare:
                        foreach (var p in points)
                        {
                            path.AddRect(new SKRect(p.X - markerSize / 2, p.Y - markerSize / 2, p.X + markerSize / 2, p.Y + markerSize / 2));
                        }
                        paint.Style = SKPaintStyle.Fill;
                        break;
                    case MarkerShape.openSquare: // TODO make it realy open, filled for now                        
                        foreach (var p in points)
                        {
                            path.AddRect(new SKRect(p.X - markerSize / 2, p.Y - markerSize / 2, p.X + markerSize / 2, p.Y + markerSize / 2));
                        }
                        paint.Style = SKPaintStyle.Stroke;
                        break;
                    case MarkerShape.filledDiamond:
                        foreach (var p in points)
                        {
                            path.MoveTo(p.X, p.Y + markerSize / 2);
                            path.LineTo(p.X - markerSize / 2, p.Y);
                            path.LineTo(p.X, p.Y - markerSize / 2);
                            path.LineTo(p.X + markerSize / 2, p.Y);
                            path.LineTo(p.X, p.Y + markerSize / 2);
                        }
                        paint.Style = SKPaintStyle.Fill;
                        break;
                    case MarkerShape.openDiamond:

                        foreach (var p in points)
                        {
                            path.MoveTo(p.X, p.Y + markerSize / 2);
                            path.LineTo(p.X - markerSize / 2, p.Y);
                            path.LineTo(p.X, p.Y - markerSize / 2);
                            path.LineTo(p.X + markerSize / 2, p.Y);
                            path.LineTo(p.X, p.Y + markerSize / 2);
                        }
                        paint.Style = SKPaintStyle.Stroke;
                        break;
                    case MarkerShape.asterisk:
                        foreach (var p in points)
                        {
                            //  |
                            path.MoveTo(p.X, p.Y + markerSize / 2);
                            path.LineTo(p.X, p.Y - markerSize / 2);
                            //  /
                            path.MoveTo(p.X - markerSize / 2, p.Y + markerSize / 4);
                            path.LineTo(p.X + markerSize / 2, p.Y - markerSize / 4);
                            //  \
                            path.MoveTo(p.X - markerSize / 2, p.Y - markerSize / 4);
                            path.LineTo(p.X + markerSize / 2, p.Y + markerSize / 4);
                        }
                        paint.Style = SKPaintStyle.Stroke;
                        break;
                    case MarkerShape.hashTag:
                        foreach (var p in points)
                        {
                            //  | .    left vertical
                            path.MoveTo(p.X - markerSize / 4, p.Y - markerSize / 2);
                            path.LineTo(p.X - markerSize / 4, p.Y + markerSize / 2);
                            //  . |    right vertical
                            path.MoveTo(p.X + markerSize / 4, p.Y - markerSize / 2);
                            path.LineTo(p.X + markerSize / 4, p.Y + markerSize / 2);
                            //  ----   upper horisontal
                            //  ....
                            path.MoveTo(p.X - markerSize / 2, p.Y - markerSize / 4);
                            path.LineTo(p.X + markerSize / 2, p.Y - markerSize / 4);
                            //  ....    lower horisontal
                            //  ----
                            path.MoveTo(p.X - markerSize / 2, p.Y + markerSize / 4);
                            path.LineTo(p.X + markerSize / 2, p.Y + markerSize / 4);
                        }
                        paint.Style = SKPaintStyle.Stroke;
                        break;
                    case MarkerShape.cross:
                        foreach (var p in points)
                        {
                            //  | 
                            path.MoveTo(p.X, p.Y - markerSize / 2);
                            path.LineTo(p.X, p.Y + markerSize / 2);
                            // -
                            path.MoveTo(p.X - markerSize / 2, p.Y);
                            path.LineTo(p.X + markerSize / 2, p.Y);
                        }
                        paint.Style = SKPaintStyle.Stroke;
                        break;
                    case MarkerShape.eks:
                        foreach (var p in points)
                        {
                            //  \
                            path.MoveTo(p.X - markerSize / 2, p.Y - markerSize / 2);
                            path.LineTo(p.X + markerSize / 2, p.Y + markerSize / 2);
                            //  /
                            path.MoveTo(p.X - markerSize / 2, p.Y + markerSize / 2);
                            path.LineTo(p.X + markerSize / 2, p.Y - markerSize / 2);
                        }
                        paint.Style = SKPaintStyle.Stroke;
                        break;
                    case MarkerShape.verticalBar:
                        foreach (var p in points)
                        {
                            //  |
                            path.MoveTo(p.X, p.Y - markerSize / 2);
                            path.LineTo(p.X, p.Y + markerSize / 2);
                        }
                        paint.Style = SKPaintStyle.Stroke;
                        break;
                    case MarkerShape.triUp:
                        foreach (var p in points)
                        {
                            //  |
                            path.MoveTo(p.X, p.Y - markerSize / 2);
                            path.LineTo(p.X, p.Y);
                            // \
                            path.LineTo(p.X - markerSize / 2, p.Y + markerSize / 2);
                            // /
                            path.MoveTo(p.X, p.Y);
                            path.LineTo(p.X + markerSize / 2, p.Y + markerSize / 2);
                        }
                        paint.Style = SKPaintStyle.Stroke;
                        break;
                    case MarkerShape.triDown:
                        foreach (var p in points)
                        {
                            //  |
                            path.MoveTo(p.X, p.Y);
                            path.LineTo(p.X, p.Y + markerSize / 2);
                            // \
                            path.MoveTo(p.X, p.Y);
                            path.LineTo(p.X - markerSize / 2, p.Y - markerSize / 2);
                            // /
                            path.MoveTo(p.X, p.Y);
                            path.LineTo(p.X + markerSize / 2, p.Y - markerSize / 2);
                        }
                        paint.Style = SKPaintStyle.Stroke;
                        break;
                    default:
                        throw new NotImplementedException($"unsupported marker type: {shape}");
                }
                canvas.DrawPath(path, paint);
            }
        }
        int state = 0;
        public void SetDrawRect(Rectangle rect)
        {
            state = canvas.Save();
            canvas.Translate(rect.Left, rect.Top);
            canvas.ClipRect(new SKRect(0, 0, rect.Width, rect.Height));
        }

        public void ClearDrawRect()
        {
            canvas.RestoreToCount(state);
        }
    }
}
