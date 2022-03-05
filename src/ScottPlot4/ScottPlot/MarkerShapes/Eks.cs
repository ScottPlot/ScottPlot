using System.Drawing;

namespace ScottPlot.MarkerShapes
{
    public struct Eks : IMarker
    {
        public void Draw(Graphics gfx, PointF center, float size, Brush brush, Pen pen)
        {
            gfx.DrawLine(pen, center.X - size, center.Y - size, center.X + size, center.Y + size);
            gfx.DrawLine(pen, center.X - size, center.Y + size, center.X + size, center.Y - size);
        }
    }
}
