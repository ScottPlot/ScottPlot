using System.Drawing;

namespace ScottPlot.MarkerShapes;

public class OpenTriangleUp : IMarker
{
    public void Draw(Graphics gfx, PointF center, float size, Brush brush, Pen pen)
    {
        PointF[] points = MarkerTools.TriangleUpPoints(center, size);
        gfx.DrawPolygon(pen, points);
    }
}
