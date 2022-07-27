namespace ScottPlot.Control;

// TODO: make interface
public class PlotActionCommander
{
    private readonly IPlotActions Actions;
    private readonly MouseState Mouse;
    private readonly KeyboardState Keyboard;
    private readonly InputBindings Bindings;

    private bool LockX => Bindings.ShouldLockX(Keyboard.PressedKeys);

    private bool LockY => Bindings.ShouldLockY(Keyboard.PressedKeys);

    private bool IsZoomingRectangle = false;

    public PlotActionCommander(IPlotActions actions, InputBindings bindings, MouseState mouse, KeyboardState keyboard)
    {
        Actions = actions;
        Mouse = mouse;
        Keyboard = keyboard;
        Bindings = bindings;
    }

    public void MouseMove(Pixel newPosition)
    {
        if (Mouse.PressedButtons.Any() && Mouse.IsDragging(newPosition))
        {
            MouseDrag(
                from: Mouse.MouseDownPosition,
                to: newPosition,
                button: Mouse.PressedButtons.First(),
                keys: Keyboard.PressedKeys,
                start: Mouse.MouseDownAxisLimits);
        }
    }

    public void MouseDrag(Pixel from, Pixel to, MouseButton button, IEnumerable<Key> keys, AxisLimits start)
    {
        bool lockY = Bindings.ShouldLockY(keys);
        bool lockX = Bindings.ShouldLockX(keys);

        if (Bindings.ShouldZoomRectangle(button, keys))
        {
            Actions.ZoomRectangle(from, to, lockX, lockY);
            IsZoomingRectangle = true;
        }
        else if (button == Bindings.DragPanButton)
        {
            Actions.DragPan(start, from, to, lockX, lockY);
        }
        else if (button == Bindings.DragZoomButton)
        {
            Actions.DragZoom(start, from, to, lockX, lockY);
        }
    }

    public void MouseUp(Pixel position, MouseButton button)
    {

        bool isDragging = Mouse.IsDragging(position);

        bool droppedZoomRectangle =
            isDragging &&
            Bindings.ShouldZoomRectangle(button, Keyboard.PressedKeys) &&
            IsZoomingRectangle;

        if (droppedZoomRectangle)
        {
            Actions.ZoomRectangleApply();
            IsZoomingRectangle = false;
        }

        // this covers the case where an extremely tiny zoom rectangle was made
        if ((isDragging == false) && (button == Bindings.ClickAutoAxisButton))
        {
            Actions.AutoScale();
        }

        if (button == Bindings.DragZoomRectangleButton)
        {
            Actions.ZoomRectangleClear();
            IsZoomingRectangle = false;
        }
    }

    public void DoubleClick()
    {
        Actions.ToggleBenchmark();
    }

    public void MouseWheelVertical(Pixel pixel, float delta)
    {
        MouseWheelDirection direction = delta > 0 ? MouseWheelDirection.Up : MouseWheelDirection.Down;

        if (Bindings.ZoomInWheelDirection.HasValue && Bindings.ZoomInWheelDirection == direction)
        {
            Actions.ZoomIn(pixel, LockX, LockY);
        }
        else if (Bindings.ZoomOutWheelDirection.HasValue && Bindings.ZoomOutWheelDirection == direction)
        {
            Actions.ZoomOut(pixel, LockX, LockY);
        }
        else if (Bindings.PanUpWheelDirection.HasValue && Bindings.PanUpWheelDirection == direction)
        {
            Actions.PanUp();
        }
        else if (Bindings.PanDownWheelDirection.HasValue && Bindings.PanDownWheelDirection == direction)
        {
            Actions.PanDown();
        }
        else if (Bindings.PanRightWheelDirection.HasValue && Bindings.PanRightWheelDirection == direction)
        {
            Actions.PanRight();
        }
        else if (Bindings.PanLeftWheelDirection.HasValue && Bindings.PanLeftWheelDirection == direction)
        {
            Actions.PanLeft();
        }
        else
        {
            return;
        }
    }
}
