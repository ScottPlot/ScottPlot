using System.Data;

namespace ScottPlot.Control;

// TODO: refactor individual actions into their own classes which inherit IControlAction

// NOTE: every action should lock the plot so its actions do not affect concurrent renders

public static class StandardActions
{
    public static void ZoomIn(IPlotControl control, Pixel pixel, LockedAxes locked)
    {
        lock (control.Plot.Sync)
        {
            double zoomInFraction = 1.15;

            double xFrac = locked.X ? 1 : zoomInFraction;
            double yFrac = locked.Y ? 1 : zoomInFraction;

            MouseZoom(control.Plot, xFrac, yFrac, pixel, control.Interaction.ChangeOpposingAxesTogether);
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

            MouseZoom(control.Plot, xFrac, yFrac, pixel, control.Interaction.ChangeOpposingAxesTogether);
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

            float pixelDeltaX = -(mouseNow.X - mouseDown.X);
            float pixelDeltaY = mouseNow.Y - mouseDown.Y;

            float scaledDeltaX = pixelDeltaX / control.Plot.ScaleFactorF;
            float scaledDeltaY = pixelDeltaY / control.Plot.ScaleFactorF;

            // restore MouseDown limits
            drag.InitialLimits.Apply(control.Plot);

            IAxis? axisUnderMouse = control.Plot.GetAxis(mouseDown);

            PixelRect dataRect = control.Plot.RenderManager.LastRender.DataRect;

            if (axisUnderMouse is not null)
            {
                if (control.Interaction.ChangeOpposingAxesTogether && axisUnderMouse.IsHorizontal())
                {
                    control.Plot.Axes.XAxes.ForEach(xAxis => xAxis.Range.PanMouse(scaledDeltaX, dataRect.Width));
                }
                else if (control.Interaction.ChangeOpposingAxesTogether && axisUnderMouse.IsVertical())
                {
                    control.Plot.Axes.YAxes.ForEach(yAxis => yAxis.Range.PanMouse(scaledDeltaY, dataRect.Height));
                }
                else
                {
                    float scaledDelta = axisUnderMouse.IsHorizontal() ? scaledDeltaX : scaledDeltaY;
                    float dataSize = axisUnderMouse.IsHorizontal() ? dataRect.Width : dataRect.Height;
                    axisUnderMouse.Range.PanMouse(scaledDelta, dataSize);
                }
            }
            else
            {
                // modify all axes
                control.Plot.Axes.XAxes.ForEach(xAxis => xAxis.Range.PanMouse(scaledDeltaX, dataRect.Width));
                control.Plot.Axes.YAxes.ForEach(yAxis => yAxis.Range.PanMouse(scaledDeltaY, dataRect.Height));
            }

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

            float pixelDeltaX = mouseNow.X - mouseDown.X;
            float pixelDeltaY = -(mouseNow.Y - mouseDown.Y);

            // restore MouseDown limits
            drag.InitialLimits.Apply(control.Plot);

            IAxis? axisUnderMouse = control.Plot.GetAxis(mouseDown);

            PixelRect lastRenderDataRect = control.Plot.RenderManager.LastRender.DataRect;

            if (axisUnderMouse is not null)
            {
                if (control.Interaction.ChangeOpposingAxesTogether && axisUnderMouse.IsHorizontal())
                {
                    control.Plot.Axes.XAxes.ForEach(xAxis => xAxis.Range.ZoomMouseDelta(pixelDeltaX, lastRenderDataRect.Width));
                }
                else if (control.Interaction.ChangeOpposingAxesTogether && axisUnderMouse.IsVertical())
                {
                    control.Plot.Axes.YAxes.ForEach(yAxis => yAxis.Range.ZoomMouseDelta(pixelDeltaY, lastRenderDataRect.Height));
                }
                else
                {
                    // modify a single axis
                    float pixelDelta = axisUnderMouse.IsHorizontal() ? pixelDeltaX : pixelDeltaY;
                    float dataSize = axisUnderMouse.IsHorizontal() ? lastRenderDataRect.Width : lastRenderDataRect.Height;
                    axisUnderMouse.Range.ZoomMouseDelta(pixelDelta, dataSize);
                }
            }
            else
            {
                // modify all axes
                control.Plot.Axes.XAxes.ForEach(xAxis => xAxis.Range.ZoomMouseDelta(pixelDeltaX, lastRenderDataRect.Width));
                control.Plot.Axes.YAxes.ForEach(yAxis => yAxis.Range.ZoomMouseDelta(pixelDeltaY, lastRenderDataRect.Height));
            }

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

            IAxis? axisUnderMouse = control.Plot.GetAxis(drag.From);
            if (axisUnderMouse is not null)
            {
                // Do not respond if the axis under the mouse has no data
                // https://github.com/ScottPlot/ScottPlot/issues/3810
                var xAxes = control.Plot.GetPlottables().Select(x => (IAxis)x.Axes.XAxis);
                var yAxes = control.Plot.GetPlottables().Select(x => (IAxis)x.Axes.YAxis);
                bool axesIsUsedByPlottables = xAxes.Contains(axisUnderMouse) || yAxes.Contains(axisUnderMouse);
                if (!axesIsUsedByPlottables)
                    return;

                // NOTE: this function only changes shape of the rectangle.
                // It doesn't modify axis limits, so no action is required on the opposite axis.
                control.Plot.ZoomRectangle.VerticalSpan = axisUnderMouse.IsHorizontal();
                control.Plot.ZoomRectangle.HorizontalSpan = axisUnderMouse.IsVertical();
            }

            double scaleFactor = control.Plot.ScaleFactor;
            control.Plot.ZoomRectangle.MouseDown = new(drag.From.X / scaleFactor, drag.From.Y / scaleFactor);
            control.Plot.ZoomRectangle.MouseUp = new(drag.To.X / scaleFactor, drag.To.Y / scaleFactor);
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
            // TODO: move axis-specific autoscaling logic into IAutoScaler

            IAxis? axisUnderMouse = control.Plot.GetAxis(pixel);

            if (axisUnderMouse is not null)
            {
                if (control.Interaction.ChangeOpposingAxesTogether)
                {
                    if (axisUnderMouse.IsHorizontal())
                    {
                        control.Plot.Axes.XAxes.ForEach(control.Plot.Axes.AutoScaleX);
                    }
                    else if (axisUnderMouse.IsVertical())
                    {
                        control.Plot.Axes.YAxes.ForEach(control.Plot.Axes.AutoScaleY);
                    }
                }
                else
                {
                    if (axisUnderMouse is IYAxis yAxisUnderMouse)
                    {
                        control.Plot.Axes.AutoScaleY(yAxisUnderMouse);
                    }
                    else if (axisUnderMouse is IXAxis xAxisUnderMouse)
                    {
                        control.Plot.Axes.AutoScaleX(xAxisUnderMouse);
                    }
                }
            }
            else
            {
                control.Plot.Axes.AutoScale();
            }

            control.Refresh();
        }
    }

    public static void ShowContextMenu(IPlotControl control, Pixel position)
    {
        control.ShowContextMenu(position);
    }

    private static void MouseZoom(Plot plot, double fracX, double fracY, Pixel pixel, bool ChangeOpposingAxesTogether)
    {
        Pixel scaledPixel = new(pixel.X / plot.ScaleFactorF, pixel.Y / plot.ScaleFactorF);
        MultiAxisLimitManager originalLimits = new(plot);
        PixelRect dataRect = plot.RenderManager.LastRender.DataRect;

        // restore MouseDown limits
        originalLimits.Apply(plot);

        var axisUnderMouse = plot.GetAxis(scaledPixel);

        if (axisUnderMouse is not null)
        {
            if (ChangeOpposingAxesTogether && axisUnderMouse.IsHorizontal())
            {
                plot.Axes.XAxes.ForEach(xAxis => xAxis.Range.ZoomFrac(fracX, xAxis.GetCoordinate(scaledPixel.X, dataRect)));
            }
            if (ChangeOpposingAxesTogether && axisUnderMouse.IsVertical())
            {
                plot.Axes.YAxes.ForEach(yAxis => yAxis.Range.ZoomFrac(fracY, yAxis.GetCoordinate(scaledPixel.Y, dataRect)));
            }
            else
            {
                double frac = axisUnderMouse.IsHorizontal() ? fracX : fracY;
                float scaledCoord = (axisUnderMouse.IsHorizontal() ? scaledPixel.X : scaledPixel.Y);
                axisUnderMouse.Range.ZoomFrac(frac, axisUnderMouse.GetCoordinate(scaledCoord, dataRect));
            }
        }
        else
        {
            // modify all axes
            plot.Axes.XAxes.ForEach(xAxis => xAxis.Range.ZoomFrac(fracX, xAxis.GetCoordinate(scaledPixel.X, dataRect)));
            plot.Axes.YAxes.ForEach(yAxis => yAxis.Range.ZoomFrac(fracY, yAxis.GetCoordinate(scaledPixel.Y, dataRect)));
        }
    }
}
