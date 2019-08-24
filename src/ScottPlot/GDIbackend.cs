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
        private  Bitmap bmp;
        private Graphics gfx;
        private PixelFormat pixelFormat;
        public GDIbackend(int width, int height, PixelFormat pixelFormat)
        {
            this.pixelFormat = pixelFormat;
            bmp = new Bitmap(width, height, pixelFormat);
            gfx = Graphics.FromImage(bmp);
            
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

        public void DrawLines(Pen pen, PointF[] linePoints)
        {
            gfx.DrawLines(pen, linePoints);
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

        public void DrawRectangle(Pen pen, Rectangle rect)
        {
            gfx.DrawRectangle(pen, rect);
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

        public void DrawString(string text, Font font, Brush brush, PointF point)
        {
            gfx.DrawString(text, font, brush, point);
        }

        public SizeF MeasureString(string text, Font font)
        {
            return gfx.MeasureString(text, font);
        }
    }
}
