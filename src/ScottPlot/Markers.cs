using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot
{
    public enum MarkerShape
    {
        none,
        filledCircle,
        filledSquare,
        openCircle,
        openSquare,
    }

    public class MarkerTools
    {
        public static void DrawMarker(Graphics gfx, PointF pixelLocation, MarkerShape shape, float size, Color color)
        {

            Pen pen = new Pen(color);
            Brush brush = new SolidBrush(color);

            PointF corner1 = new PointF(pixelLocation.X - size / 2, pixelLocation.Y - size / 2);
            PointF corner2 = new PointF(pixelLocation.X + size / 2, pixelLocation.Y + size / 2);
            SizeF bounds = new SizeF(size, size);
            RectangleF rect = new RectangleF(corner1, bounds);

            switch (shape)
            {
                case MarkerShape.filledCircle:
                    gfx.FillEllipse(brush, rect);
                    break;
                case MarkerShape.openCircle:
                    gfx.DrawEllipse(pen, rect);
                    break;
                case MarkerShape.filledSquare:
                    gfx.FillRectangle(brush, rect);
                    break;
                case MarkerShape.openSquare:
                    gfx.DrawRectangle(pen, corner1.X, corner1.Y, size, size);
                    break;
                default:
                    throw new NotImplementedException($"unsupported marker type: {shape}");
            }
        }
    }
}
