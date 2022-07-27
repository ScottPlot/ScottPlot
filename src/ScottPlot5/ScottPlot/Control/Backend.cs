namespace ScottPlot.Control;

public class Backend
{
    public InputBindings InputBindings { get; set; } = InputBindings.Standard();

    public IPlotActions PlotActions { get; private set; }

    private readonly Plot Plot;

    private readonly KeyboardState Keyboard = new();

    private readonly MouseState Mouse = new();

    private bool LockX => InputBindings.ShouldLockX(Keyboard.PressedKeys);

    private bool LockY => InputBindings.ShouldLockY(Keyboard.PressedKeys);

    public Backend(IPlotControl control)
    {
        Plot = control.Plot;
        PlotActions = new PlotActions(control);
    }

    public Coordinates GetMouseCoordinates(Axes.IAxis? xAxis = null, Axes.IAxis? yAxis = null)
    {
        return Plot.GetCoordinate(Mouse.LastPosition, xAxis, yAxis);
    }

    public void MouseDown(Pixel position, MouseButton button)
    {
        Mouse.Down(position, button, Plot.GetAxisLimits());
    }

    public void MouseUp(Pixel position, MouseButton button)
    {
        bool isDragging = Mouse.IsDragging(position);

        bool droppedZoomRectangle =
            isDragging &&
            InputBindings.ShouldZoomRectangle(button, Keyboard.PressedKeys) &&
            Plot.ZoomRectangle.IsVisible;

        if (droppedZoomRectangle)
        {
            PlotActions.ZoomRectangleApply();
        }

        // this covers the case where an extremely tiny zoom rectangle was made
        if ((isDragging == false) && (button == InputBindings.ClickAutoAxisButton))
        {
            PlotActions.AutoScale();
        }

        if (button == InputBindings.DragZoomRectangleButton)
        {
            PlotActions.ZoomRectangleClear();
        }

        Mouse.Up(button);
    }

    public void MouseMove(Pixel newPosition)
    {
        Mouse.LastPosition = newPosition;

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
        bool lockY = InputBindings.ShouldLockY(keys);
        bool lockX = InputBindings.ShouldLockX(keys);

        if (InputBindings.ShouldZoomRectangle(button, keys))
        {
            PlotActions.ZoomRectangle(from, to, lockX, lockY);
        }
        else if (button == InputBindings.DragPanButton)
        {
            PlotActions.DragPan(start, from, to, lockX, lockY);
        }
        else if (button == InputBindings.DragZoomButton)
        {
            PlotActions.DragZoom(start, from, to, lockX, lockY);
        }
    }

    public void DoubleClick()
    {
        PlotActions.ToggleBenchmark();
    }

    public void MouseWheelVertical(Pixel pixel, float delta)
    {
        MouseWheelDirection direction = delta > 0 ? MouseWheelDirection.Up : MouseWheelDirection.Down;

        if (InputBindings.ZoomInWheelDirection.HasValue && InputBindings.ZoomInWheelDirection == direction)
        {
            PlotActions.ZoomIn(pixel, LockX, LockY);
        }
        else if (InputBindings.ZoomOutWheelDirection.HasValue && InputBindings.ZoomOutWheelDirection == direction)
        {
            PlotActions.ZoomOut(pixel, LockX, LockY);
        }
        else if (InputBindings.PanUpWheelDirection.HasValue && InputBindings.PanUpWheelDirection == direction)
        {
            PlotActions.PanUp();
        }
        else if (InputBindings.PanDownWheelDirection.HasValue && InputBindings.PanDownWheelDirection == direction)
        {
            PlotActions.PanDown();
        }
        else if (InputBindings.PanRightWheelDirection.HasValue && InputBindings.PanRightWheelDirection == direction)
        {
            PlotActions.PanRight();
        }
        else if (InputBindings.PanLeftWheelDirection.HasValue && InputBindings.PanLeftWheelDirection == direction)
        {
            PlotActions.PanLeft();
        }
        else
        {
            return;
        }
    }

    public void KeyDown(Key key)
    {
        Keyboard.Down(key);
    }

    public void KeyUp(Key key)
    {
        Keyboard.Up(key);
    }
}
