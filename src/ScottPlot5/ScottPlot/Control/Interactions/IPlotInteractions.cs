using ScottPlot.Control.EventArgs;

namespace ScottPlot.Control.Interactions;

/// <summary>
/// Anyone who wants to create a custom set of interactions must implement this interface
/// </summary>
public interface IPlotInteractions
{
    public void MouseDown(IPlotControl sender, MouseDownEventArgs e);
    public void MouseUp(IPlotControl sender, MouseUpEventArgs e);
    public void MouseMove(IPlotControl sender, MouseMoveEventArgs e);
    public void MouseDrag(IPlotControl sender, MouseDragEventArgs e);
    public void DoubleClick(IPlotControl sender, MouseDownEventArgs e);
    public void MouseWheel(IPlotControl sender, MouseWheelEventArgs e);
    public void MouseDragEnd(IPlotControl sender, MouseDragEventArgs e);
    public void KeyDown(IPlotControl sender, KeyDownEventArgs e);
    public void KeyUp(IPlotControl sender, KeyUpEventArgs e);
}
