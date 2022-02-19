using System.Drawing;

namespace ScottPlot.MarkerShapes
{
    public class Cross : IMarker
    {
        public void Draw(Graphics gfx, PointF center, float size, Brush brush, Pen pen)
        {
            RectangleF rect = new(center.X - size, center.Y - size, center.X + size, center.Y + size);
            float centerX = rect.Left + rect.Width / 2;
            float centerY = rect.Top + rect.Height / 2;

            gfx.DrawLine(pen, rect.Left, centerY, rect.Right, centerY);
            gfx.DrawLine(pen, centerX, rect.Top, centerX, rect.Bottom);
        }
    }
}
