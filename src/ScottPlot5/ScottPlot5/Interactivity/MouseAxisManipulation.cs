namespace ScottPlot.Interactivity;

/// <summary>
/// This class stores logic for changing axis limits according to mouse inputs in pixel units.
/// </summary>
public static class MouseAxisManipulation
{
    public static void MouseWheelZoom(Plot plot, double fracX, double fracY, Pixel pixel, bool ChangeOpposingAxesTogether)
    {
        Pixel scaledPixel = pixel.Divide(plot.ScaleFactorF);
        ScottPlot.Control.MultiAxisLimitManager originalLimits = new(plot);
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

    public static void DragPan(Plot plot, Pixel mouseDown, Pixel mouseNow)
    {
        IPlotControl control = plot.PlotControl ?? throw new NullReferenceException();

        float pixelDeltaX = -(mouseNow.X - mouseDown.X);
        float pixelDeltaY = mouseNow.Y - mouseDown.Y;

        float scaledDeltaX = pixelDeltaX / plot.ScaleFactorF;
        float scaledDeltaY = pixelDeltaY / plot.ScaleFactorF;

        IAxis? axisUnderMouse = plot.GetAxis(mouseDown);

        PixelRect dataRect = plot.RenderManager.LastRender.DataRect;

        if (axisUnderMouse is not null)
        {
            if (control.Interaction.ChangeOpposingAxesTogether && axisUnderMouse.IsHorizontal())
            {
                plot.Axes.XAxes.ForEach(xAxis => xAxis.Range.PanMouse(scaledDeltaX, dataRect.Width));
            }
            else if (control.Interaction.ChangeOpposingAxesTogether && axisUnderMouse.IsVertical())
            {
                plot.Axes.YAxes.ForEach(yAxis => yAxis.Range.PanMouse(scaledDeltaY, dataRect.Height));
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
            plot.Axes.XAxes.ForEach(xAxis => xAxis.Range.PanMouse(scaledDeltaX, dataRect.Width));
            plot.Axes.YAxes.ForEach(yAxis => yAxis.Range.PanMouse(scaledDeltaY, dataRect.Height));
        }
    }

    public static void DragZoom(Plot plot, Pixel mouseDown, Pixel mouseNow)
    {
        IPlotControl control = plot.PlotControl ?? throw new NullReferenceException();

        float pixelDeltaX = mouseNow.X - mouseDown.X;
        float pixelDeltaY = -(mouseNow.Y - mouseDown.Y);

        IAxis? axisUnderMouse = plot.GetAxis(mouseDown);

        PixelRect lastRenderDataRect = plot.RenderManager.LastRender.DataRect;

        if (axisUnderMouse is not null)
        {
            if (control.Interaction.ChangeOpposingAxesTogether && axisUnderMouse.IsHorizontal())
            {
                plot.Axes.XAxes.ForEach(xAxis => xAxis.Range.ZoomMouseDelta(pixelDeltaX, lastRenderDataRect.Width));
            }
            else if (control.Interaction.ChangeOpposingAxesTogether && axisUnderMouse.IsVertical())
            {
                plot.Axes.YAxes.ForEach(yAxis => yAxis.Range.ZoomMouseDelta(pixelDeltaY, lastRenderDataRect.Height));
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
            plot.Axes.XAxes.ForEach(xAxis => xAxis.Range.ZoomMouseDelta(pixelDeltaX, lastRenderDataRect.Width));
            plot.Axes.YAxes.ForEach(yAxis => yAxis.Range.ZoomMouseDelta(pixelDeltaY, lastRenderDataRect.Height));
        }
    }

    public static void PlaceZoomRectangle(Plot plot, Pixel px1, Pixel px2)
    {
        float scaleFactor = plot.ScaleFactorF;

        IAxis? axisUnderMouse = plot.GetAxis(px1.Divide(scaleFactor));
        if (axisUnderMouse is not null)
        {
            // Do not respond if the axis under the mouse has no data
            // https://github.com/ScottPlot/ScottPlot/issues/3810
            var xAxes = plot.GetPlottables().Select(x => (IAxis)x.Axes.XAxis);
            var yAxes = plot.GetPlottables().Select(x => (IAxis)x.Axes.YAxis);
            bool axesIsUsedByPlottables = xAxes.Contains(axisUnderMouse) || yAxes.Contains(axisUnderMouse);
            if (!axesIsUsedByPlottables)
                return;

            // NOTE: this function only changes shape of the rectangle.
            // It doesn't modify axis limits, so no action is required on the opposite axis.
            plot.ZoomRectangle.VerticalSpan = axisUnderMouse.IsHorizontal();
            plot.ZoomRectangle.HorizontalSpan = axisUnderMouse.IsVertical();
        }

        plot.ZoomRectangle.MouseDown = px1.Divide(scaleFactor);
        plot.ZoomRectangle.MouseUp = px2.Divide(scaleFactor);
    }

    public static void AutoScale(Plot plot, Pixel pixel, bool allParallelAxes)
    {
        IAxis? axisUnderMouse = plot.GetAxis(pixel);

        if (axisUnderMouse is null)
        {
            plot.Axes.AutoScale();
            return;
        }

        if (allParallelAxes)
        {
            if (axisUnderMouse.IsHorizontal())
            {
                plot.Axes.XAxes.ForEach(plot.Axes.AutoScaleX);
            }
            else if (axisUnderMouse.IsVertical())
            {
                plot.Axes.YAxes.ForEach(plot.Axes.AutoScaleY);
            }
        }
        else
        {
            if (axisUnderMouse is IYAxis yAxisUnderMouse)
            {
                plot.Axes.AutoScaleY(yAxisUnderMouse);
            }
            else if (axisUnderMouse is IXAxis xAxisUnderMouse)
            {
                plot.Axes.AutoScaleX(xAxisUnderMouse);
            }
        }
    }
}
