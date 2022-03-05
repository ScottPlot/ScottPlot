using System.Drawing;

namespace ScottPlot.MarkerShapes
{
    public struct TriStarDown : IMarker
    {
        public void Draw(Graphics gfx, PointF center, float size, Brush brush, Pen pen)
        {
            PointF[] points = MarkerTools.TriangleDownPoints(center, size);
            MarkerTools.DrawRadial(gfx, pen, center, points);
        }
    }
}
