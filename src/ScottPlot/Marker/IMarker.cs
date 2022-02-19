using System.Drawing;

namespace ScottPlot
{
    public interface IMarker
    {
        public void Draw(Graphics gfx, PointF center, float size, Brush brush, Pen pen);
    }
}
