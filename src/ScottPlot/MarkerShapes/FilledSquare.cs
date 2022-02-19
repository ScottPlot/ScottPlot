using System.Drawing;

namespace ScottPlot.MarkerShapes
{
    public struct FilledSquare : IMarker
    {
        public void Draw(Graphics gfx, PointF center, float radius, Brush brush, Pen pen)
        {
            RectangleF rect = new(center.X - radius, center.Y - radius, radius * 2, radius * 2);
            gfx.FillRectangle(brush, rect);
        }
    }
}
