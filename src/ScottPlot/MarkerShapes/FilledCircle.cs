using System.Drawing;

namespace ScottPlot.MarkerShapes;

public class FilledCircle : IMarker
{
    public void Draw(Graphics gfx, PointF center, float size, Brush brush, Pen pen)
    {
        RectangleF rect = new(center.X - size, center.Y - size, center.X + size, center.Y + size);
        gfx.FillEllipse(brush, rect);
    }
}
