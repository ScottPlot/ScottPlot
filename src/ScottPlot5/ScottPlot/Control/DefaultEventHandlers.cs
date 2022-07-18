using ScottPlot.Control.EventArgs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ScottPlot.Control
{
    internal static class DefaultEventHandlers
    {
        public static void MouseDown(Plot plot, MouseDownEventArgs e, Action requestRender)
        {
        }

        public static void MouseUp(Plot plot, MouseUpEventArgs e, Action requestRender)
        {
            if (e.Handled)
                return;


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

        public static void MouseMove(Plot plot, MouseMoveEventArgs e, Action requestRender)
        {

        }

        public static void MouseDrag(Plot plot, MouseDragEventArgs e, Action requestRender)
        {
            if (e.Handled)
                return;

            switch (e.Button)
            {
                case MouseButton.Mouse1:
                    if (e.PressedKeys.Contains(Key.Alt))
                    {
                        plot.MouseZoomRectangle(e.From, e.To);
                    }
                    else
                    {
                        Pixel panTo = e.To;
                        panTo.X = e.PressedKeys.Contains(Key.Shift) ? e.From.X : panTo.X;
                        panTo.Y = e.PressedKeys.Contains(Key.Ctrl) ? e.From.Y : panTo.Y;

                        plot.MousePan(e.MouseDown.AxisLimits, e.From, panTo);
                    }
                    break;
                case MouseButton.Mouse2:
                    plot.MouseZoom(e.MouseDown.AxisLimits, e.From, e.To);
                    break;
                case MouseButton.Mouse3:
                    plot.MouseZoomRectangle(e.From, e.To);
                    break;
                default:
                    return;
            }

            requestRender();
        }

        public static void DoubleClick(Plot plot, MouseDownEventArgs e, Action requestRender)
        {
            if (e.Handled)
                return;

            plot.Benchmark.IsVisible = !plot.Benchmark.IsVisible;

            requestRender();
        }

        public static void MouseWheel(Plot plot, MouseWheelEventArgs e, Action requestRender)
        {
            if (e.Handled)
                return;

            double fracX = e.DeltaY > 0 ? 1.15 : .85;
            double fracY = e.DeltaY > 0 ? 1.15 : .85;
            plot.MouseZoom(fracX, fracY, e.Position);

            requestRender();
        }

        public static void MouseDragEnd(Plot plot, MouseDragEventArgs e, Action requestRender)
        {
            if (e.Handled)
                return;

            if (e.Button == MouseButton.Mouse3 || (e.Button == MouseButton.Mouse1 && e.PressedKeys.Contains(Key.Alt)))
            {
                plot.MouseZoomRectangleClear(applyZoom: true);
                requestRender();
            }
        }

        public static void KeyDown(Plot plot, KeyDownEventArgs e, Action requestRender)
        {
        }

        public static void KeyUp(Plot plot, KeyUpEventArgs e, Action requestRender)
        {
        }
    }
}
