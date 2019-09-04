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

        public void DrawMarkers(PointF[] points, MarkerShape shape, float markerSize, Color color)
        {
            var paint = new SKPaint() { Color = color.ToSKColor(), IsAntialias = AA, StrokeWidth = markerSize / 5 };
            switch (shape)
            {
                case MarkerShape.filledCircle:
                    paint.StrokeCap = SKStrokeCap.Round;
                    paint.StrokeWidth = markerSize;
                    canvas.DrawPoints(SKPointMode.Points, points.Select(x => x.ToSKPoint()).ToArray(), paint);
                    break;
                case MarkerShape.openCircle:
                    var pathOpenCircle = new SKPath();
                    foreach (var p in points)
                    {
                        pathOpenCircle.AddCircle(p.X, p.Y, markerSize / 2);
                    }
                    paint.Style = SKPaintStyle.Stroke;
                    canvas.DrawPath(pathOpenCircle, paint);
                    break;
                case MarkerShape.filledSquare:
                    paint.StrokeCap = SKStrokeCap.Square;
                    paint.StrokeWidth = markerSize;
                    canvas.DrawPoints(SKPointMode.Points, points.Select(x => x.ToSKPoint()).ToArray(), paint);
                    break;
                case MarkerShape.openSquare: // TODO make it realy open, filled for now
                    var pathOpenSquare = new SKPath();
                    foreach (var p in points)
                    {
                        pathOpenSquare.MoveTo(p.X - markerSize / 2, p.Y - markerSize / 2);
                        pathOpenSquare.LineTo(p.X - markerSize / 2, p.Y + markerSize / 2);
                        pathOpenSquare.LineTo(p.X + markerSize / 2, p.Y + markerSize / 2);
                        pathOpenSquare.LineTo(p.X + markerSize / 2, p.Y - markerSize / 2);
                        pathOpenSquare.LineTo(p.X - markerSize / 2, p.Y - markerSize / 2);
                    }
                    paint.Style = SKPaintStyle.Stroke;
                    canvas.DrawPath(pathOpenSquare, paint);
                    break;
                case MarkerShape.filledDiamond:
                    var path = new SKPath();
                    foreach (var p in points)
                    {
                        path.MoveTo(p.X, p.Y + markerSize / 2);
                        path.LineTo(p.X - markerSize / 2, p.Y);
                        path.LineTo(p.X, p.Y - markerSize / 2);
                        path.LineTo(p.X + markerSize / 2, p.Y);
                        path.LineTo(p.X, p.Y + markerSize / 2);
                    }
                    canvas.DrawPath(path, paint);
                    break;
                case MarkerShape.openDiamond:
                    var path2 = new SKPath();
                    foreach (var p in points)
                    {
                        path2.MoveTo(p.X, p.Y + markerSize / 2);
                        path2.LineTo(p.X - markerSize / 2, p.Y);
                        path2.LineTo(p.X, p.Y - markerSize / 2);
                        path2.LineTo(p.X + markerSize / 2, p.Y);
                        path2.LineTo(p.X, p.Y + markerSize / 2);
                    }
                    paint.Style = SKPaintStyle.Stroke;
                    canvas.DrawPath(path2, paint);
                    break;
                case MarkerShape.asterisk:
                    var path3 = new SKPath();
                    foreach (var p in points)
                    {
                        //  |
                        path3.MoveTo(p.X, p.Y + markerSize / 2);
                        path3.LineTo(p.X, p.Y - markerSize / 2);
                        //  /
                        path3.MoveTo(p.X - markerSize / 2, p.Y + markerSize / 4);
                        path3.LineTo(p.X + markerSize / 2, p.Y - markerSize / 4);
                        //  \
                        path3.MoveTo(p.X - markerSize / 2, p.Y - markerSize / 4);
                        path3.LineTo(p.X + markerSize / 2, p.Y + markerSize / 4);
                    }
                    paint.Style = SKPaintStyle.Stroke;
                    canvas.DrawPath(path3, paint);
                    break;
                case MarkerShape.hashTag:
                    var path4 = new SKPath();
                    foreach (var p in points)
                    {
                        //  | .    left vertical
                        path4.MoveTo(p.X - markerSize / 4, p.Y - markerSize / 2);
                        path4.LineTo(p.X - markerSize / 4, p.Y + markerSize / 2);
                        //  . |    right vertical
                        path4.MoveTo(p.X + markerSize / 4, p.Y - markerSize / 2);
                        path4.LineTo(p.X + markerSize / 4, p.Y + markerSize / 2);
                        //  ----   upper horisontal
                        //  ....
                        path4.MoveTo(p.X - markerSize / 2, p.Y - markerSize / 4);
                        path4.LineTo(p.X + markerSize / 2, p.Y - markerSize / 4);
                        //  ....    lower horisontal
                        //  ----
                        path4.MoveTo(p.X - markerSize / 2, p.Y + markerSize / 4);
                        path4.LineTo(p.X + markerSize / 2, p.Y + markerSize / 4);

                    }
                    paint.Style = SKPaintStyle.Stroke;
                    canvas.DrawPath(path4, paint);
                    break;
                case MarkerShape.cross:
                    var path5 = new SKPath();
                    foreach (var p in points)
                    {
                        //  | 
                        path5.MoveTo(p.X, p.Y - markerSize / 2);
                        path5.LineTo(p.X, p.Y + markerSize / 2);
                        // -
                        path5.MoveTo(p.X - markerSize / 2, p.Y);
                        path5.LineTo(p.X + markerSize / 2, p.Y);
                    }
                    paint.Style = SKPaintStyle.Stroke;
                    canvas.DrawPath(path5, paint);
                    break;
                case MarkerShape.eks:
                    var path6 = new SKPath();
                    foreach (var p in points)
                    {
                        //  \
                        path6.MoveTo(p.X - markerSize / 2, p.Y - markerSize / 2);
                        path6.LineTo(p.X + markerSize / 2, p.Y + markerSize / 2);
                        //  /
                        path6.MoveTo(p.X - markerSize / 2, p.Y + markerSize / 2);
                        path6.LineTo(p.X + markerSize / 2, p.Y - markerSize / 2);
                    }
                    paint.Style = SKPaintStyle.Stroke;
                    canvas.DrawPath(path6, paint);
                    break;
                case MarkerShape.verticalBar:
                    var path7 = new SKPath();
                    foreach (var p in points)
                    {
                        //  |
                        path7.MoveTo(p.X, p.Y - markerSize / 2);
                        path7.LineTo(p.X, p.Y + markerSize / 2);
                    }
                    paint.Style = SKPaintStyle.Stroke;
                    canvas.DrawPath(path7, paint);
                    break;
                case MarkerShape.triUp:
                    var path8 = new SKPath();
                    foreach (var p in points)
                    {
                        //  |
                        path8.MoveTo(p.X, p.Y - markerSize / 2);
                        path8.LineTo(p.X, p.Y);
                        // \
                        path8.LineTo(p.X - markerSize / 2, p.Y + markerSize / 2);
                        // /
                        path8.MoveTo(p.X, p.Y);
                        path8.LineTo(p.X + markerSize / 2, p.Y + markerSize / 2);
                    }
                    paint.Style = SKPaintStyle.Stroke;
                    canvas.DrawPath(path8, paint);
                    break;
                case MarkerShape.triDown:
                    var path9 = new SKPath();
                    foreach (var p in points)
                    {
                        //  |
                        path9.MoveTo(p.X, p.Y + markerSize / 2);
                        path9.LineTo(p.X, p.Y);
                        // \
                        path9.LineTo(p.X - markerSize / 2, p.Y - markerSize / 2);
                        // /
                        path9.MoveTo(p.X, p.Y);
                        path9.LineTo(p.X + markerSize / 2, p.Y - markerSize / 2);
                    }
                    paint.Style = SKPaintStyle.Stroke;
                    canvas.DrawPath(path9, paint);
                    break;
                default:
                    throw new NotImplementedException($"unsupported marker type: {shape}");
            }
        }
    }
}
