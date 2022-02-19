using System.Drawing;

namespace ScottPlot.MarkerShapes
{
    public struct HorizontalBar : IMarker
    {
        public void Draw(Graphics gfx, PointF center, float size, Brush brush, Pen pen)
        {
            gfx.DrawLine(pen, center.X - size, center.Y, center.X + size, center.Y);
        }
    }
}
