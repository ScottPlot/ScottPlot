using System.Drawing;

namespace ScottPlot.MarkerShapes;

public class VerticalBar : IMarker
{
    public void Draw(Graphics gfx, PointF center, float size, Brush brush, Pen pen)
    {
        gfx.DrawLine(pen, center.X, center.Y - size, center.X, center.Y + size);
    }
}
