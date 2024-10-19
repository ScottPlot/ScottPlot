using ScottPlot.Interactivity;

namespace ScottPlot.Control;

#pragma warning disable CS0618 // disable obsolete Interaction warning
public static class StandardActions
{
    public static void ZoomIn(IPlotControl control, Pixel pixel, LockedAxes locked)
    {
        lock (control.Plot.Sync)
        {
            double zoomInFraction = 1.15;

            double xFrac = locked.X ? 1 : zoomInFraction;
            double yFrac = locked.Y ? 1 : zoomInFraction;

            Interactivity.MouseAxisManipulation.MouseWheelZoom(control.Plot, xFrac, yFrac, pixel, control.Interaction.ChangeOpposingAxesTogether);
            control.Refresh();
        }
    }

    public static void ZoomOut(IPlotControl control, Pixel pixel, LockedAxes locked)
    {
        lock (control.Plot.Sync)
        {
            double zoomOutFraction = 0.85;

            double xFrac = locked.X ? 1 : zoomOutFraction;
            double yFrac = locked.Y ? 1 : zoomOutFraction;

            Interactivity.MouseAxisManipulation.MouseWheelZoom(control.Plot, xFrac, yFrac, pixel, control.Interaction.ChangeOpposingAxesTogether);
            control.Refresh();
        }
    }

    public static void PanUp(IPlotControl control)
    {
        lock (control.Plot.Sync)
        {
            double PanFraction = 0.1;
            AxisLimits limits = control.Plot.Axes.GetLimits();
            double deltaY = limits.Rect.Height * PanFraction;
            control.Plot.Axes.SetLimits(limits.WithPan(0, deltaY));
            control.Refresh();
        }
    }

    public static void PanDown(IPlotControl control)
    {
        lock (control.Plot.Sync)
        {
            double PanFraction = 0.1;
            AxisLimits limits = control.Plot.Axes.GetLimits();
            double deltaY = limits.Rect.Height * PanFraction;
            control.Plot.Axes.SetLimits(limits.WithPan(0, -deltaY));
            control.Refresh();
        }
    }

    public static void PanLeft(IPlotControl control)
    {
        lock (control.Plot.Sync)
        {
            double PanFraction = 0.1;
            AxisLimits limits = control.Plot.Axes.GetLimits();
            double deltaX = limits.Rect.Width * PanFraction;
            control.Plot.Axes.SetLimits(limits.WithPan(-deltaX, 0));
            control.Refresh();
        }
    }

    public static void PanRight(IPlotControl control)
    {
        lock (control.Plot.Sync)
        {
            double PanFraction = 0.1;
            AxisLimits limits = control.Plot.Axes.GetLimits();
            double deltaX = limits.Rect.Width * PanFraction;
            control.Plot.Axes.SetLimits(limits.WithPan(deltaX, 0));
            control.Refresh();
        }
    }

    public static void DragPan(IPlotControl control, MouseDrag drag, LockedAxes locked)
    {
        lock (control.Plot.Sync)
        {
            Pixel mouseNow = new(
                x: locked.X ? drag.From.X : drag.To.X,
                y: locked.Y ? drag.From.Y : drag.To.Y);

            Pixel mouseDown = drag.From;

            drag.InitialLimits.Apply(control.Plot);
            Interactivity.MouseAxisManipulation.DragPan(control.Plot, mouseDown, mouseNow);
            control.Refresh();
        }
    }

    public static void DragZoom(IPlotControl control, MouseDrag drag, LockedAxes locked)
    {
        lock (control.Plot.Sync)
        {
            Pixel mouseNow = new(
                x: locked.X ? drag.From.X : drag.To.X,
                y: locked.Y ? drag.From.Y : drag.To.Y);

            Pixel mouseDown = drag.From;

            // restore MouseDown limits
            drag.InitialLimits.Apply(control.Plot);
            Interactivity.MouseAxisManipulation.DragZoom(control.Plot, mouseDown, mouseNow);
            control.Refresh();
        }
    }

    public static void ZoomRectangleClear(IPlotControl control)
    {
        lock (control.Plot.Sync)
        {
            control.Plot.ZoomRectangle.IsVisible = false;
            control.Refresh();
        }
    }

    public static void ZoomRectangleApply(IPlotControl control)
    {
        lock (control.Plot.Sync)
        {
            IAxis? axisUnderMouse = control.Plot.GetAxis(control.Plot.ZoomRectangle.MouseDown);

            if (axisUnderMouse is not null)
            {
                if (control.Interaction.ChangeOpposingAxesTogether && axisUnderMouse.IsHorizontal())
                {
                    control.Plot.Axes.XAxes.ForEach(control.Plot.ZoomRectangle.Apply);
                }
                else if (control.Interaction.ChangeOpposingAxesTogether && axisUnderMouse.IsVertical())
                {
                    control.Plot.Axes.YAxes.ForEach(control.Plot.ZoomRectangle.Apply);
                }
                else if (axisUnderMouse is IXAxis xAxis)
                {
                    control.Plot.ZoomRectangle.Apply(xAxis);
                }
                else if (axisUnderMouse is IYAxis yAxis)
                {
                    control.Plot.ZoomRectangle.Apply(yAxis);
                }
            }
            else
            {
                control.Plot.Axes.XAxes.ForEach(control.Plot.ZoomRectangle.Apply);
                control.Plot.Axes.YAxes.ForEach(control.Plot.ZoomRectangle.Apply);
            }

            control.Refresh();
        }
    }

    public static void DragZoomRectangle(IPlotControl control, MouseDrag drag, LockedAxes locked)
    {
        lock (control.Plot.Sync)
        {
            control.Plot.ZoomRectangle.VerticalSpan = locked.X;
            control.Plot.ZoomRectangle.HorizontalSpan = locked.Y;
            Interactivity.MouseAxisManipulation.PlaceZoomRectangle(control.Plot, drag.From, drag.To);
            control.Plot.ZoomRectangle.IsVisible = true;
            control.Refresh();
        }
    }

    public static void ToggleBenchmark(IPlotControl control)
    {
        lock (control.Plot.Sync)
        {
            control.Plot.Benchmark.IsVisible = !control.Plot.Benchmark.IsVisible;
            control.Refresh();
        }
    }

    public static void AutoScale(IPlotControl control, Pixel pixel)
    {
        lock (control.Plot.Sync)
        {
            MouseAxisManipulation.AutoScale(control.Plot, pixel, control.Interaction.ChangeOpposingAxesTogether);
            control.Refresh();
        }
    }

    public static void ShowContextMenu(IPlotControl control, Pixel position)
    {
        control.ShowContextMenu(position);
    }
}
