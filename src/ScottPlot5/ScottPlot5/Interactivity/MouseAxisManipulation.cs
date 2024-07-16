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

        float scaledDeltaX = pixelDeltaX / control.Plot.ScaleFactorF;
        float scaledDeltaY = pixelDeltaY / control.Plot.ScaleFactorF;

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
    }

    public static void DragZoom(Plot plot, Pixel mouseDown, Pixel mouseNow)
    {
        IPlotControl control = plot.PlotControl ?? throw new NullReferenceException();

        float pixelDeltaX = mouseNow.X - mouseDown.X;
        float pixelDeltaY = -(mouseNow.Y - mouseDown.Y);

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
    }
}
