using System.Drawing;

namespace ScottPlot.MarkerShapes
{
    public struct FilledDiamond : IMarker
    {
        public void Draw(Graphics gfx, PointF center, float size, Brush brush, Pen pen)
        {
            PointF[] points = MarkerTools.DiamondPoints(center, size);
            gfx.FillPolygon(brush, points);
        }
    }
}
