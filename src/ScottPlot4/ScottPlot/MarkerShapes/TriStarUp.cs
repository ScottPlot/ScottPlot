using System.Drawing;

namespace ScottPlot.MarkerShapes
{
    public struct TriStarUp : IMarker
    {
        public void Draw(Graphics gfx, PointF center, float size, Brush brush, Pen pen)
        {
            PointF[] points = MarkerTools.TriangleUpPoints(center, size);
            MarkerTools.DrawRadial(gfx, pen, center, points);
        }
    }
}
