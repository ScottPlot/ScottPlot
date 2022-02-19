using System.Drawing;

namespace ScottPlot.MarkerShapes
{
    public struct VerticalBar : IMarker
    {
        public void Draw(Graphics gfx, PointF center, float size, Brush brush, Pen pen)
        {
            gfx.DrawLine(pen, center.X, center.Y - size, center.X, center.Y + size);
        }
    }
}
