using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot
{
    public interface IGraphicBackend
    {
        Size GetSize();
        void SetAntiAlias(bool enabled);
        Bitmap GetBitmap();
        void Resize(int width, int height);
        void Clear(Color color);
        void RotateTransform(float angle);
        void ResetRotateTransform();

        void DrawImage(Image image, Point point);
        void DrawLine(Pen pen, int x1, int y1, int x2, int y2);
        void DrawLine(Pen pen, float x1, float y1, float x2, float y2);
        void DrawLine(Pen pen, Point start, Point end);
        void DrawLine(Pen pen, PointF start, PointF end);
        void DrawLines(Pen pen, PointF[] linePoints);
        void DrawLinesPaired(Pen pen, PointF[] linePoints);
        void FillCircles(Brush brush, PointF[] points, float radius);
        void FillEllipse(Brush brush, float x, float y, float widht, float height);

        void FillEllipse(Brush brush, RectangleF rect);
        void DrawEllipse(Pen pen, RectangleF rect);
        void FillRectangle(Brush brush, RectangleF rect);
        void FillRectangles(Brush brush, RectangleF[] rects);
        void FillRectangle(Brush brush, float x, float y, float widht, float height);
        void DrawRectangle(Pen pen, Rectangle rect);
        void DrawRectangles(Pen pen, RectangleF[] rects);
        void DrawRectangle(Pen pen, float x, float y, float width, float height);
        void FillPolygon(Brush brush, PointF[] curvePoints);
        void DrawPolygon(Pen pen, PointF[] curvePoints);
        void DrawString(string text, Font font, Brush brush, PointF point, StringFormat format);
        void DrawString(string text, Font font, Brush brush, PointF point);
        SizeF MeasureString(string text, Font font);
        void DrawMarkers(PointF[] points, MarkerShape shape, float markerSize, Color color);
        void SetDrawRect(Rectangle rect);
        void ClearDrawRect();
    }
}
