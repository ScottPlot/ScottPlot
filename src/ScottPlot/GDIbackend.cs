using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot
{
    public class GDIbackend : IGraphicBackend
    {
        private Bitmap bmp;
        private Graphics gfx;
        private PixelFormat pixelFormat;
        public GDIbackend(int width, int height, PixelFormat pixelFormat)
        {
            this.pixelFormat = pixelFormat;
            bmp = new Bitmap(width, height, pixelFormat);
            gfx = Graphics.FromImage(bmp);

        }

        public float DpiX => gfx.DpiX;
        public float DpiY => gfx.DpiY;
        public void RotateTransform(float angle)
        {
            gfx.RotateTransform(angle);
        }
        public void ResetRotateTransform()
        {
            gfx.ResetTransform();
        }

        public void DrawImage(Image image, Point point)
        {
            gfx.DrawImage(image, point);
        }
        public Size GetSize()
        {
            return bmp.Size;
        }
        public void SetAntiAlias(bool enabled)
        {
            if (enabled)
            {
                gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            }
            else
            {
                gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
            }
        }

        public Bitmap GetBitmap()
        {
            return bmp;
        }

        public void Resize(int width, int height)
        {
            if (width > 0 && height > 0)
            {
                bmp = new Bitmap(width, height, pixelFormat);
                gfx = Graphics.FromImage(bmp);
            }
        }

        public void Clear(Color color)
        {
            gfx.Clear(color);
        }
        public void DrawLine(Pen pen, Point start, Point end)
        {
            gfx.DrawLine(pen, start, end);
        }
        public void DrawLine(Pen pen, PointF start, PointF end)
        {
            gfx.DrawLine(pen, start, end);
        }
        public void DrawLine(Pen pen, int x1, int y1, int x2, int y2)
        {
            gfx.DrawLine(pen, x1, y1, x2, y2);
        }

        public void DrawLine(Pen pen, float x1, float y1, float x2, float y2)
        {
            gfx.DrawLine(pen, x1, y1, x2, y2);
        }

        public void FillCircles(Brush brush, PointF[] points, float radius)
        {
            foreach (var point in points)
            {
                gfx.FillEllipse(brush, point.X - radius / 2, point.Y - radius / 2, radius, radius);
            }
        }

        public void DrawLines(Pen pen, PointF[] linePoints)
        {
            gfx.DrawLines(pen, linePoints);
        }

        public void DrawLinesPaired(Pen pen, PointF[] linePoints)
        {
            for (int i = 0; i < linePoints.Length; i += 2)
            {
                gfx.DrawLine(pen, linePoints[i], linePoints[i + 1]);
            }
        }

        public void FillEllipse(Brush brush, float x, float y, float widht, float height)
        {
            gfx.FillEllipse(brush, x, y, widht, height);
        }

        public void FillEllipse(Brush brush, RectangleF rect)
        {
            gfx.FillEllipse(brush, rect);
        }

        public void DrawEllipse(Pen pen, RectangleF rect)
        {
            gfx.DrawEllipse(pen, rect);
        }

        public void FillRectangle(Brush brush, RectangleF rect)
        {
            gfx.FillRectangle(brush, rect);
        }

        public void FillRectangle(Brush brush, float x, float y, float width, float height)
        {
            gfx.FillRectangle(brush, x, y, width, height);
        }

        public void FillRectangles(Brush brush, RectangleF[] rects)
        {
            foreach (var rect in rects)
                gfx.FillRectangle(brush, rect);
        }

        public void DrawRectangle(Pen pen, Rectangle rect)
        {
            gfx.DrawRectangle(pen, rect);
        }
        public void DrawRectangles(Pen pen, RectangleF[] rects)
        {
            gfx.DrawRectangles(pen, rects);
        }

        public void DrawRectangle(Pen pen, float x, float y, float width, float height)
        {
            gfx.DrawRectangle(pen, x, y, width, height);
        }

        public void FillPolygon(Brush brush, PointF[] curvePoints)
        {
            gfx.FillPolygon(brush, curvePoints);
        }

        public void DrawPolygon(Pen pen, PointF[] curvePoints)
        {
            gfx.DrawPolygon(pen, curvePoints);
        }

        public void DrawString(string text, Font font, Brush brush, PointF point, StringFormat format)
        {
            gfx.DrawString(text, font, brush, point, format);
        }

        public void DrawString(string text, Font font, Brush brush, PointF point)
        {
            gfx.DrawString(text, font, brush, point);
        }

        public SizeF MeasureString(string text, Font font)
        {
            return gfx.MeasureString(text, font);
        }

        public void DrawMarkers(PointF[] points, MarkerShape shape, float markerSize, Color color)
        {
            Pen pen = new Pen(color);
            Brush brush = new SolidBrush(color);
            IEnumerable<RectangleF> rects = points.Select(p => new RectangleF(p.X - markerSize / 2, p.Y - markerSize / 2, markerSize, markerSize));
            switch (shape)
            {
                case MarkerShape.filledCircle:
                    foreach (var rect in rects)
                        FillEllipse(brush, rect);
                    break;
                case MarkerShape.openCircle:
                    foreach (var rect in rects)
                        DrawEllipse(pen, rect);
                    break;
                case MarkerShape.filledSquare:
                    foreach (var rect in rects)
                        FillRectangle(brush, rect);
                    break;
                case MarkerShape.openSquare:
                    DrawRectangles(pen, rects.ToArray());
                    break;
                case MarkerShape.filledDiamond:
                    PointF[] curvePoints =
                    {
                        new PointF(0, markerSize /2),
                        new PointF(-markerSize/2, 0),
                        new PointF(0, - markerSize/2),
                        new PointF(markerSize/2, 0)
                    };
                    var polys = points.Select(p => curvePoints.Select(cp => new PointF(p.X + cp.X, p.Y + cp.Y)));
                    foreach (var poly in polys)
                        FillPolygon(brush, poly.ToArray());
                    break;
                case MarkerShape.openDiamond:
                    PointF[] curvePoints2 =
                    {
                        new PointF(0, markerSize /2),
                        new PointF(-markerSize/2, 0),
                        new PointF(0, - markerSize/2),
                        new PointF(markerSize/2, 0)
                    };
                    var polys1 = points.Select(p => curvePoints2.Select(cp => new PointF(p.X + cp.X, p.Y + cp.Y)));
                    foreach (var poly in polys1)
                        DrawPolygon(pen, poly.ToArray());
                    break;
                case MarkerShape.asterisk:
                    Font drawFont = new Font("CourierNew", markerSize * 3);
                    SizeF textSize = MeasureString("*", drawFont);
                    var asteriskPoints = points.Select(p => new PointF(p.X - textSize.Width / 2, p.Y - textSize.Height / 4));
                    foreach (var aPoint in asteriskPoints)
                        DrawString("*", drawFont, brush, aPoint);
                    break;
                case MarkerShape.hashTag:
                    Font drawFont2 = new Font("CourierNew", markerSize * 2);
                    SizeF textSize2 = MeasureString("#", drawFont2);
                    var asteriskPoints2 = points.Select(p => new PointF(p.X - textSize2.Width / 2, p.Y - textSize2.Height / 3));
                    foreach (var aPoint in asteriskPoints2)
                        DrawString("#", drawFont2, brush, aPoint);
                    break;
                case MarkerShape.cross:
                    Font drawFont3 = new Font("CourierNew", markerSize * 2);
                    SizeF textSize3 = MeasureString("+", drawFont3);
                    var asteriskPoints3 = points.Select(p => new PointF(p.X - textSize3.Width / (5 / 2), p.Y - textSize3.Height / 2));
                    foreach (var aPoint in asteriskPoints3)
                        DrawString("+", drawFont3, brush, aPoint);
                    break;
                case MarkerShape.eks:
                    Font drawFont4 = new Font("CourierNew", markerSize * 2);
                    SizeF textSize4 = MeasureString("x", drawFont4);
                    var asteriskPoints4 = points.Select(p => new PointF(p.X - textSize4.Width / (5 / 2), p.Y - textSize4.Height / 2));
                    foreach (var aPoint in asteriskPoints4)
                        DrawString("x", drawFont4, brush, aPoint);
                    break;
                case MarkerShape.verticalBar:
                    Font drawFont5 = new Font("CourierNew", markerSize * 2);
                    SizeF textSize5 = MeasureString("|", drawFont5);
                    var asteriskPoints5 = points.Select(p => new PointF(p.X - textSize5.Width / (5 / 2), p.Y - textSize5.Height / 2));
                    foreach (var aPoint in asteriskPoints5)
                        DrawString("|", drawFont5, brush, aPoint);
                    break;
                case MarkerShape.triUp:
                    // Create points that define polygon.
                    PointF[] curvePoints3 =
                    {
                        new PointF(0,0),
                        new PointF(0, -markerSize),
                        new PointF(0, 0),
                        new PointF(0 - markerSize * 0.866f, markerSize * 0.5f),
                        new PointF(0,0),
                        new PointF(0 + markerSize * 0.866f, markerSize * 0.5f)
                    };
                    var polys3 = points.Select(p => curvePoints3.Select(cp => new PointF(p.X + cp.X, p.Y + cp.Y)));

                    //Draw polygon to screen
                    foreach (var poly in polys3)
                        DrawPolygon(pen, poly.ToArray());
                    break;
                case MarkerShape.triDown:
                    // Create points that define polygon.
                    PointF[] curvePoints4 =
                    {
                        new PointF(0, 0),
                        new PointF(0, markerSize),
                        new PointF(0, 0),
                        new PointF(-markerSize * 0.866f, -markerSize * 0.5f),
                        new PointF(0, 0),
                        new PointF(markerSize * 0.866f, -markerSize * 0.5f)
                    };
                    var polys4 = points.Select(p => curvePoints4.Select(cp => new PointF(p.X + cp.X, p.Y + cp.Y)));
                    //Draw polygons to screen
                    foreach (var poly in polys4)
                        DrawPolygon(pen, poly.ToArray());
                    break;
                default:
                    throw new NotImplementedException($"unsupported marker type: {shape}");
            }
        }

        public void SetDrawRect(Rectangle rect)
        {
            return;
        }

        public void ClearDrawRect()
        {
            return;
        }
    }
}
