namespace ScottPlot.Control;

public interface IPlotActions
{
    public void DragPan(AxisLimits start, Pixel from, Pixel to, bool lockX, bool lockY);
    public void DragZoom(AxisLimits start, Pixel from, Pixel to, bool lockX, bool lockY);

    public void ZoomIn(Pixel pixel, bool lockX, bool lockY);
    public void ZoomOut(Pixel pixel, bool lockX, bool lockY);

    public void PanUp();
    public void PanDown();
    public void PanLeft();
    public void PanRight();

    public void ZoomRectangle(Pixel from, Pixel to, bool lockX, bool lockY);
    public void ZoomRectangleApply();
    public void ZoomRectangleClear();

    public void ToggleBenchmark();

    public void AutoScale();
}
