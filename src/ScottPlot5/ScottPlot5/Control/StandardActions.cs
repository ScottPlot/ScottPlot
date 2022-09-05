using ScottPlot.Axis;

namespace ScottPlot.Control;

public static class StandardActions
{
    public static void ZoomIn(IPlotControl control, Pixel pixel, LockedAxes locked)
    {
        double zoomInFraction = 1.15;

        double xFrac = locked.X ? 1 : zoomInFraction;
        double yFrac = locked.Y ? 1 : zoomInFraction;

        control.Plot.MouseZoom(xFrac, yFrac, pixel);
        control.Refresh();
    }

    public static void ZoomOut(IPlotControl control, Pixel pixel, LockedAxes locked)
    {
        double zoomOutFraction = 0.85;

        double xFrac = locked.X ? 1 : zoomOutFraction;
        double yFrac = locked.Y ? 1 : zoomOutFraction;

        control.Plot.MouseZoom(xFrac, yFrac, pixel);
        control.Refresh();
    }

    public static void PanUp(IPlotControl control)
    {
        double PanFraction = 0.1;
        AxisLimits limits = control.Plot.GetAxisLimits();
        double deltaY = limits.Rect.Height * PanFraction;
        control.Plot.SetAxisLimits(limits.WithPan(0, deltaY));
        control.Refresh();
    }

    public static void PanDown(IPlotControl control)
    {
        double PanFraction = 0.1;
        AxisLimits limits = control.Plot.GetAxisLimits();
        double deltaY = limits.Rect.Height * PanFraction;
        control.Plot.SetAxisLimits(limits.WithPan(0, -deltaY));
        control.Refresh();
    }

    public static void PanLeft(IPlotControl control)
    {
        double PanFraction = 0.1;
        AxisLimits limits = control.Plot.GetAxisLimits();
        double deltaX = limits.Rect.Width * PanFraction;
        control.Plot.SetAxisLimits(limits.WithPan(-deltaX, 0));
        control.Refresh();
    }

    public static void PanRight(IPlotControl control)
    {
        double PanFraction = 0.1;
        AxisLimits limits = control.Plot.GetAxisLimits();
        double deltaX = limits.Rect.Width * PanFraction;
        control.Plot.SetAxisLimits(limits.WithPan(deltaX, 0));
        control.Refresh();
    }

    public static void DragPan(IPlotControl control, MouseDrag drag, LockedAxes locked)
    {
        Pixel to2 = new(
            x: locked.X ? drag.From.X : drag.To.X,
            y: locked.Y ? drag.From.Y : drag.To.Y);

        control.Plot.MousePan(drag.InitialLimits, drag.From, to2);
        control.Refresh();
    }

    public static void DragZoom(IPlotControl control, MouseDrag drag, LockedAxes locked)
    {
        Pixel to2 = new(
            x: locked.X ? drag.From.X : drag.To.X,
            y: locked.Y ? drag.From.Y : drag.To.Y);

        control.Plot.MouseZoom(drag.InitialLimits, drag.From, to2);
        control.Refresh();
    }

    public static void ZoomRectangleClear(IPlotControl control)
    {
        control.Plot.ZoomRectangle.IsVisible = false;
        control.Refresh();
    }

    public static void ZoomRectangleApply(IPlotControl control)
    {
        Pixel px1 = control.Plot.ZoomRectangle.MouseDown;
        Pixel px2 = control.Plot.ZoomRectangle.MouseUp;
        PixelRect dataRect = control.Plot.LastRenderInfo.DataRect;

        foreach (IXAxis xAxis in control.Plot.XAxes)
        {
            double x1 = xAxis.GetCoordinate(px1.X, dataRect);
            double x2 = xAxis.GetCoordinate(px2.X, dataRect);
            double xMin = Math.Min(x1, x2);
            double xMax = Math.Max(x1, x2);
            xAxis.Range.Set(xMin, xMax);
        }

        foreach (IYAxis yAxis in control.Plot.YAxes)
        {
            double y1 = yAxis.GetCoordinate(px1.Y, dataRect);
            double y2 = yAxis.GetCoordinate(px2.Y, dataRect);
            double xMin = Math.Min(y1, y2);
            double xMax = Math.Max(y1, y2);
            yAxis.Range.Set(xMin, xMax);
        }

        control.Refresh();
    }

    public static void DragZoomRectangle(IPlotControl control, MouseDrag drag, LockedAxes locked)
    {
        control.Plot.MouseZoomRectangle(drag.From, drag.To, vSpan: locked.Y, hSpan: locked.X);
        control.Refresh();
    }

    public static void ToggleBenchmark(IPlotControl control)
    {
        control.Plot.Benchmark.IsVisible = !control.Plot.Benchmark.IsVisible;
        control.Refresh();
    }

    public static void AutoScale(IPlotControl control)
    {
        control.Plot.AutoScale();
        control.Refresh();
    }
}
