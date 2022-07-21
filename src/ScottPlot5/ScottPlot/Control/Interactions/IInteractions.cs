using ScottPlot.Control.EventArgs;

namespace ScottPlot.Control.Interactions;

/// <summary>
/// Implementers define how user interactions act on the given <see cref="IPlotControl"/>
/// </summary>
public interface IInteractions
{
    IPlotControl Control { get; }
    public void MouseDown(Pixel pixel, MouseButton button);
    public void MouseUp(Pixel pixel, MouseButton button, bool endDrag);
    public void MouseMove(Pixel pixel);
    public void MouseDrag(Pixel from, Pixel to, MouseButton button, IEnumerable<Key> keys, AxisLimits start);
    public void DoubleClick();
    public void MouseWheel(Pixel pixel, float delta);
    public void MouseDragEnd(MouseButton button, IEnumerable<Key> keys);
    public void KeyDown(Key key);
    public void KeyUp(Key key);
}
