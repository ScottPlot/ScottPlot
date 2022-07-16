using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ScottPlot.Control
{
    internal static class DefaultEventHandlers
    {
        public static void MouseDown(Plot plot, MouseInteraction e, Action requestRender)
        {
        }

        public static void MouseUp(Plot plot, MouseInteraction e, Action requestRender)
        {
        }

        public static void MouseMove(Plot plot, MouseInteraction e, Action requestRender)
        {
            
        }
        public static void MouseDrag(Plot plot, MouseDragInteraction e, Action requestRender)
        {
            if (e.Button == MouseButton.Mouse1)
            {
                plot.MousePan(e.MouseDown.AxisLimits, e.From, e.To);
            }
            
            if (e.Button == MouseButton.Mouse2)
            {
                plot.MouseZoom(e.MouseDown.AxisLimits, e.From, e.To);
            }
            
            if (e.Button == MouseButton.Mouse3)
            {
                plot.MouseZoomRectangle(e.From, e.To);
            }

            requestRender();
        }
    }
}
