using System.Drawing;

namespace ScottPlot.MarkerShapes
{
    public struct Hashtag : IMarker
    {
        public void Draw(Graphics gfx, PointF center, float radius, Brush brush, Pen pen)
        {
            RectangleF rect = new(center.X - radius, center.Y - radius, radius * 2, radius * 2);

            float centerX1 = rect.Left + rect.Width * .33f;
            float centerX2 = rect.Left + rect.Width * .66f;
            float centerY1 = rect.Top + rect.Height * .33f;
            float centerY2 = rect.Top + rect.Height * .66f;

            gfx.DrawLine(pen, centerX1, rect.Bottom, centerX1, rect.Top);
            gfx.DrawLine(pen, centerX2, rect.Bottom, centerX2, rect.Top);
            gfx.DrawLine(pen, rect.Left, centerY1, rect.Right, centerY1);
            gfx.DrawLine(pen, rect.Left, centerY2, rect.Right, centerY2);
        }
    }
}
