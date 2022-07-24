namespace ScottPlot.Control;

/// <summary>
/// This class maps common plot actions with specific mouse and keyboard inputs
/// </summary>
public class InputMap
{
    public MouseButton? ClickDragPan = null;
    public MouseButton? ClickDragZoom = null;
    public MouseButton? ClickDragZoomRectangle = null;
    public MouseButton? DoubleClick = null;
    public MouseButton? ZoomIn = null;
    public MouseButton? ZoomOut = null;

    public Key? LockHorizontalAxis = null;
    public Key? LockVerticalAxis = null;
    public Key? PanZoomRectangle = null;

    public bool IsLockedX(IEnumerable<Key> keys)
    {
        return LockHorizontalAxis.HasValue ? keys.Contains(LockHorizontalAxis.Value) : false;
    }

    public bool IsLockedY(IEnumerable<Key> keys)
    {
        return LockVerticalAxis.HasValue ? keys.Contains(LockVerticalAxis.Value) : false;
    }

    public bool IsZoomingRectangle(MouseButton button, IEnumerable<Key> keys)
    {
        if (button == ClickDragZoomRectangle)
        {
            return true;
        }

        if (button == ClickDragPan)
        {
            if (PanZoomRectangle.HasValue && keys.Contains(PanZoomRectangle.Value))
            {
                return true;
            }
        }

        return false;
    }

    public static InputMap Standard() => new()
    {
        ClickDragPan = MouseButton.Left,
        ClickDragZoomRectangle = MouseButton.Middle,
        ClickDragZoom = MouseButton.Right,
        DoubleClick = MouseButton.Left,
        ZoomIn = MouseButton.WheelUp,
        ZoomOut = MouseButton.WheelDown,
        LockHorizontalAxis = Key.Shift,
        LockVerticalAxis = Key.Ctrl,
        PanZoomRectangle = Key.Alt,
    };

    public static InputMap NonInteractive() => new() { };

    public static InputMap Alternate() => new()
    {
        ClickDragPan = MouseButton.Middle,
        ClickDragZoomRectangle = MouseButton.Left,
        ZoomIn = MouseButton.WheelUp,
        ZoomOut = MouseButton.WheelDown,
    };

    public static InputMap WheelOnly() => new()
    {
        ClickDragPan = MouseButton.Middle,
        ZoomIn = MouseButton.WheelUp,
        ZoomOut = MouseButton.WheelDown,
    };
}
