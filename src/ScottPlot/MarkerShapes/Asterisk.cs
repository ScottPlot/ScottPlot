using System.Drawing;

namespace ScottPlot.MarkerShapes;

public class Asterisk : IMarker
{
    public void Draw(Graphics gfx, PointF center, float size, Brush brush, Pen pen)
    {
        RectangleF rect = new(center.X - size, center.Y - size, center.X + size, center.Y + size);
        float centerX = rect.Left + rect.Width / 2;
        float centerY = rect.Top + rect.Height / 2;
        float dsize = 0.707f * rect.Width / 2;

        gfx.DrawLine(pen, centerX, rect.Bottom, centerX, rect.Top);
        gfx.DrawLine(pen, rect.Left, centerY, rect.Right, centerY);
        gfx.DrawLine(pen, centerX - dsize, centerY - dsize, centerX + dsize, centerY + dsize);
        gfx.DrawLine(pen, centerX - dsize, centerY + dsize, centerX + dsize, centerY - dsize);
    }
}
