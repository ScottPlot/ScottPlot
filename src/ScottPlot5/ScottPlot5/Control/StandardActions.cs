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

            MouseZoom(control.Plot, xFrac, yFrac, pixel);
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

            MouseZoom(control.Plot, xFrac, yFrac, pixel);
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
                // modify a single axis
                float scaledDelta = axisUnderMouse.IsHorizontal() ? scaledDeltaX : scaledDeltaY;
                float dataSize = axisUnderMouse.IsHorizontal() ? dataRect.Width : dataRect.Height;
                axisUnderMouse.Range.PanMouse(scaledDelta, dataSize);
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
                // modify a single axis
                float pixelDelta = axisUnderMouse.IsHorizontal() ? pixelDeltaX : pixelDeltaY;
                float dataSize = axisUnderMouse.IsHorizontal() ? lastRenderDataRect.Width : lastRenderDataRect.Height;
                axisUnderMouse.Range.ZoomMouseDelta(pixelDelta, dataSize);
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
            control.Plot.ZoomRectangle.Apply(control.Plot);
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
                if (axisUnderMouse is IYAxis yAxisUnderMouse)
                {
                    control.Plot.Axes.AutoScaleY(yAxisUnderMouse);
                }
                else if (axisUnderMouse is IXAxis xAxisUnderMouse)
                {
                    control.Plot.Axes.AutoScaleX(xAxisUnderMouse);
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

    private static void MouseZoom(Plot plot, double fracX, double fracY, Pixel pixel)
    {
        MultiAxisLimitManager originalLimits = new(plot);
        PixelRect dataRect = plot.RenderManager.LastRender.DataRect;

        // restore MouseDown limits
        originalLimits.Apply(plot);

        var axisUnderMouse = plot.GetAxis(pixel);

        if (axisUnderMouse is not null)
        {
            // modify a single axis
            double frac = axisUnderMouse.IsHorizontal() ? fracX : fracY;
            float scaledCoord = (axisUnderMouse.IsHorizontal() ? pixel.X : pixel.Y) / plot.ScaleFactorF;
            axisUnderMouse.Range.ZoomFrac(frac, axisUnderMouse.GetCoordinate(scaledCoord, dataRect));
        }
        else
        {
            // modify all axes
            Pixel scaledPixel = new(pixel.X / plot.ScaleFactor, pixel.Y / plot.ScaleFactor);
            plot.Axes.XAxes.ForEach(xAxis => xAxis.Range.ZoomFrac(fracX, xAxis.GetCoordinate(scaledPixel.X, dataRect)));
            plot.Axes.YAxes.ForEach(yAxis => yAxis.Range.ZoomFrac(fracY, yAxis.GetCoordinate(scaledPixel.Y, dataRect)));
        }
    }
}
