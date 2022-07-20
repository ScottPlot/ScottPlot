using ScottPlot.Control.EventArgs;

namespace ScottPlot.Control.Interactions;

/// <summary>
/// Implementers define how user interactions act on the given <see cref="IPlotControl"/>
/// </summary>
public interface IInteractions
{
    IPlotControl Control { get; }
    public void MouseDown(MouseDownEventArgs e);
    public void MouseUp(MouseUpEventArgs e);
    public void MouseMove(MouseMoveEventArgs e);
    public void MouseDrag(MouseDragEventArgs e);
    public void DoubleClick(MouseDownEventArgs e);
    public void MouseWheel(MouseWheelEventArgs e);
    public void MouseDragEnd(MouseDragEventArgs e);
    public void KeyDown(KeyDownEventArgs e);
    public void KeyUp(KeyUpEventArgs e);
}
