using System.Drawing;

namespace ScottPlot.MarkerShapes
{
    public struct OpenSquare : IMarker
    {
        public void Draw(Graphics gfx, PointF center, float radius, Brush brush, Pen pen)
        {
            RectangleF rect = new(center.X - radius, center.Y - radius, radius * 2, radius * 2);
            gfx.DrawRectangle(pen, rect.Left, rect.Top, rect.Width, rect.Height);
        }
    }
}
