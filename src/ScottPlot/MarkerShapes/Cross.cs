using System.Drawing;

namespace ScottPlot.MarkerShapes
{
    public struct Cross : IMarker
    {
        public void Draw(Graphics gfx, PointF center, float radius, Brush brush, Pen pen)
        {
            RectangleF rect = new(center.X - radius, center.Y - radius, radius * 2, radius * 2);
            float centerX = rect.Left + rect.Width / 2;
            float centerY = rect.Top + rect.Height / 2;

            gfx.DrawLine(pen, rect.Left, centerY, rect.Right, centerY);
            gfx.DrawLine(pen, centerX, rect.Top, centerX, rect.Bottom);
        }
    }
}
