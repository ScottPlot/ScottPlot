namespace ScottPlot.Control.Interactions;

/// <summary>
/// Implementers define how user interactions act on the given <see cref="IPlotControl"/>
/// </summary>
public interface IInteractions
{
    IPlotControl Control { get; }
    public InputMap InputMap { get; set; }
    public void MouseDown(Pixel pixel, MouseButton button);
    public void MouseUp(Pixel pixel, MouseButton button, bool drag);
    public void MouseWheel(Pixel pixel, MouseWheelDirection direction, IEnumerable<Key> keys);
    public void MouseMove(Pixel pixel);
    public void MouseDrag(Pixel from, Pixel to, MouseButton button, IEnumerable<Key> keys, AxisLimits start);
    public void DoubleClick();
    public void MouseDragEnd(MouseButton button, IEnumerable<Key> keys);
    public void KeyDown(Key key);
    public void KeyUp(Key key);
}
