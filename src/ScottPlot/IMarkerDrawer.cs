using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot
{
    public interface IMarkerDrawer
    {
        void DrawMarker(Graphics gfx, PointF pixelLocation, MarkerShape shape, float size, Color color);
    }
}
