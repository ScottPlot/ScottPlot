using System.Drawing;

namespace ScottPlot.MarkerShapes
{
    public struct OpenTriangleDown : IMarker
    {
        public void Draw(Graphics gfx, PointF center, float size, Brush brush, Pen pen)
        {
            PointF[] points = MarkerTools.TriangleDownPoints(center, size);
            gfx.DrawPolygon(pen, points);
        }
    }
}
