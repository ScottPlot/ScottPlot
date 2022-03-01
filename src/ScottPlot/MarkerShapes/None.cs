using System.Drawing;

namespace ScottPlot.MarkerShapes
{
    public struct None : IMarker
    {
        public void Draw(Graphics gfx, PointF center, float size, Brush brush, Pen pen)
        {
        }
    }
}
