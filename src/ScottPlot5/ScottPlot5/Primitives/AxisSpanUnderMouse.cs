namespace ScottPlot;

public class AxisSpanUnderMouse
{
    public required Plottables.AxisSpan Span;
    public required Coordinates MouseStart;
    public required CoordinateRange OriginalRange;
    public bool ResizeEdge1;
    public bool ResizeEdge2;
    public bool IsResizing => ResizeEdge1 || ResizeEdge2;
    public bool IsMoving => !IsResizing;
    public bool IsResizingVertically => IsResizing && Span is Plottables.VerticalSpan;
    public bool IsResizingHorizontally => IsResizing && Span is Plottables.HorizontalSpan;
    public void DragTo(Coordinates mouseNow) => Span.DragTo(this, mouseNow);
}
