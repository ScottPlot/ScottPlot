using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ScottPlot.Control
{
    internal static class DefaultEventHandlers
    {
        public static void MouseDown(Plot plot, MouseDownInteraction e, Action requestRender)
        {
        }

        public static void MouseUp(Plot plot, MouseUpInteraction e, Action requestRender)
        {
            switch (e.Button)
            {
                case MouseButton.Mouse3:
                    if (!e.CancelledDrag)
                    {
                        plot.MouseZoomRectangleClear(applyZoom: false);
                        plot.AutoScale();
                    }
                    break;
                default:
                    return;
            }

            requestRender();
        }

        public static void MouseMove(Plot plot, MouseDownInteraction e, Action requestRender)
        {
            
        }
        
        public static void MouseDrag(Plot plot, MouseDragInteraction e, Action requestRender, Action<Action> setOnDragRelease)
        {
            switch (e.Button)
            {
                case MouseButton.Mouse1:
                    plot.MousePan(e.MouseDown.AxisLimits, e.From, e.To);
                    break;
                case MouseButton.Mouse2:
                    plot.MouseZoom(e.MouseDown.AxisLimits, e.From, e.To);
                    break;
                case MouseButton.Mouse3:
                    plot.MouseZoomRectangle(e.From, e.To);
                    setOnDragRelease(() => plot.MouseZoomRectangleClear(applyZoom: true));
                    break;
                default: 
                    return;
            }
            
            requestRender();
        }
    }
}
