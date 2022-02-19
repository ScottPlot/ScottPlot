using System.Drawing;

namespace ScottPlot.MarkerShapes
{
    public class FilledSquare : IMarker
    {
        public void Draw(Graphics gfx, PointF center, float size, Brush brush, Pen pen)
        {
            RectangleF rect = new(center.X - size, center.Y - size, center.X + size, center.Y + size);
            gfx.FillRectangle(brush, rect);
        }
    }
}
