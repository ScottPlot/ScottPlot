using System.Drawing;

namespace ScottPlot.MarkerShapes
{
    public struct FilledTriangleUp : IMarker
    {
        public void Draw(Graphics gfx, PointF center, float size, Brush brush, Pen pen)
        {
            PointF[] points = MarkerTools.TriangleUpPoints(center, size);
            gfx.FillPolygon(brush, points);
        }
    }
}
