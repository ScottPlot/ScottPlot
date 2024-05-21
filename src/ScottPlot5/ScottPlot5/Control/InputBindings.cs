namespace ScottPlot.Control;

/// <summary>
/// This class defines which buttons and keys perform which actions to manipulate the plot.
/// </summary>
public class InputBindings
{
    public MouseButton? DragPanButton = null;
    public MouseButton? DragZoomButton = null;
    public MouseButton? DragZoomRectangleButton = null;
    public MouseButton? DoubleClickButton = null;
    public MouseButton? ClickAutoAxisButton = null;
    public MouseButton? ClickContextMenuButton = null;

    public MouseWheelDirection? ZoomInWheelDirection = null;
    public MouseWheelDirection? ZoomOutWheelDirection = null;
    public MouseWheelDirection? PanUpWheelDirection = null;
    public MouseWheelDirection? PanDownWheelDirection = null;
    public MouseWheelDirection? PanLeftWheelDirection = null;
    public MouseWheelDirection? PanRightWheelDirection = null;

    public Key? LockHorizontalAxisKey = null;
    public Key? LockVerticalAxisKey = null;
    public Key? PanZoomRectangleKey = null;

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="keys"/> contains the key which locks the X axis
    /// </summary>
    public virtual bool ShouldLockX(IEnumerable<Key> keys, MouseButton? button = null)
    {
        return LockHorizontalAxisKey.HasValue ? keys.Contains(LockHorizontalAxisKey.Value) : false;
    }

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="keys"/> contains the key which locks the Y axis
    /// </summary>
    public virtual bool ShouldLockY(IEnumerable<Key> keys, MouseButton? button = null)
    {
        return LockVerticalAxisKey.HasValue ? keys.Contains(LockVerticalAxisKey.Value) : false;
    }

    /// <summary>
    /// Returns <see langword="true"/> if the combination of pressed buttons and keys results in a click-drag zoom rectangle
    /// </summary>
    public virtual bool ShouldZoomRectangle(MouseButton button, IEnumerable<Key> keys)
    {
        if (button == DragZoomRectangleButton)
        {
            return true;
        }

        if (button == DragPanButton)
        {
            if (PanZoomRectangleKey.HasValue && keys.Contains(PanZoomRectangleKey.Value))
            {
                return true;
            }
        }

        return false;
    }

    public static InputBindings Standard() => new()
    {
        DragPanButton = MouseButton.Left,
        DragZoomRectangleButton = MouseButton.Middle,
        DragZoomButton = MouseButton.Right,
        ClickAutoAxisButton = MouseButton.Middle,
        ClickContextMenuButton = MouseButton.Right,
        DoubleClickButton = MouseButton.Left,
        ZoomInWheelDirection = MouseWheelDirection.Up,
        ZoomOutWheelDirection = MouseWheelDirection.Down,
        LockHorizontalAxisKey = Key.Shift,
        LockVerticalAxisKey = Key.Ctrl,
        PanZoomRectangleKey = Key.Alt,
    };

    public static InputBindings NonInteractive() => new() { };
}
