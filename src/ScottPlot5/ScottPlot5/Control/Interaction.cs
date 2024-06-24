namespace ScottPlot.Control;

/// <summary>
/// This class contains logic to perform plot manipulations in response to UI actions.
/// To customize how user inputs are interpreted, inherit and override functions in this class.
/// To customize behavior of actions, replace properties of <see cref="Actions"/> with custom delegates.
/// To customize UI inputs, assign desired button and key properties of <see cref="Inputs"/>.
/// </summary>
public class Interaction(IPlotControl control) : IPlotInteraction
{
    public IPlotControl PlotControl { get; private set; } = control;

    /// <summary>
    /// Indicates whether interactions have been disabled.
    /// </summary>
    public bool Disabled { get; private set; } = false;

    /// <summary>
    /// Buttons and keys in this object can be overwritten to customize actions for specific user input events.
    /// (e.g., make left-click-drag zoom instead of pan)
    /// </summary>
    public InputBindings Inputs = InputBindings.Standard();

    /// <summary>
    /// Stores the <see cref="Actions"/> that were present when <see cref="Disable"/> was called.
    /// </summary>
    private PlotActions ActionsWhenDisabled = PlotActions.Standard();

    /// <summary>
    /// Delegates in this object can be overwritten with custom functions that manipulate the plot.
    /// (e.g., changing the sensitivity of click-drag-zooming)
    /// </summary>
    public PlotActions Actions = PlotActions.Standard();

    protected readonly KeyboardState Keyboard = new();
    protected readonly MouseState Mouse = new();

    public bool IsDraggingMouse(Pixel pos) => Mouse.PressedButtons.Any() && Mouse.IsDragging(pos);
    protected bool LockX => Inputs.ShouldLockX(Keyboard.PressedKeys);
    protected bool LockY => Inputs.ShouldLockY(Keyboard.PressedKeys);
    protected bool IsZoomingRectangle = false;
    public bool ChangeOpposingAxesTogether { get; set; } = false;

    /// <summary>
    /// Disable all mouse interactivity
    /// </summary>
    public void Disable()
    {
        if (Disabled)
            return;

        Disabled = true;
        ActionsWhenDisabled = Actions;
        Actions = PlotActions.NonInteractive();
    }

    /// <summary>
    /// Enable mouse interactivity using the default mouse actions
    /// </summary>
    public void Enable()
    {
        if (Disabled)
            Actions = ActionsWhenDisabled;

        Disabled = false;
    }

    /// <summary>
    /// Enable mouse interactivity using custom mouse actions
    /// </summary>
    public void Enable(PlotActions customActions)
    {
        Actions = customActions;
        Disabled = false;
    }

    /// <summary>
    /// Return the last observed location of the mouse in coordinate units
    /// </summary>
    public Coordinates GetMouseCoordinates(IXAxis? xAxis = null, IYAxis? yAxis = null)
    {
        return PlotControl.Plot.GetCoordinates(Mouse.LastPosition, xAxis, yAxis);
    }

    public virtual void OnMouseMove(Pixel newPosition)
    {
        Mouse.LastPosition = newPosition;

        if (IsDraggingMouse(newPosition))
        {
            MouseDrag(
                from: Mouse.MouseDownPosition,
                to: newPosition,
                button: Mouse.PressedButtons.First(),
                keys: Keyboard.PressedKeys,
                start: Mouse.MouseDownAxisLimits);
        }
    }

    protected virtual void MouseDrag(Pixel from, Pixel to, MouseButton button, IEnumerable<Key> keys, MultiAxisLimitManager start)
    {
        bool lockY = Inputs.ShouldLockY(keys, button);
        bool lockX = Inputs.ShouldLockX(keys, button);
        LockedAxes locks = new(lockX, lockY);

        MouseDrag drag = new(start, from, to);

        if (Inputs.ShouldZoomRectangle(button, keys) || IsZoomingRectangle)
        {
            Actions.DragZoomRectangle(PlotControl, drag, locks);
            IsZoomingRectangle = true;
        }
        else if (button == Inputs.DragPanButton)
        {
            Actions.DragPan(PlotControl, drag, locks);
        }
        else if (button == Inputs.DragZoomButton)
        {
            Actions.DragZoom(PlotControl, drag, locks);
        }
    }

    public virtual void KeyUp(Key key)
    {
        Keyboard.Up(key);
    }

    public virtual void KeyDown(Key key)
    {
        Keyboard.Down(key);
    }

    public virtual void MouseDown(Pixel position, MouseButton button)
    {
        Mouse.Down(position, button, new MultiAxisLimitManager(PlotControl));
    }

    public virtual void MouseUp(Pixel position, MouseButton button)
    {
        bool isDragging = Mouse.IsDragging(position);
        bool droppedZoomRectangle = isDragging && IsZoomingRectangle;

        if (droppedZoomRectangle)
        {
            Actions.ZoomRectangleApply(PlotControl);
            Actions.ZoomRectangleClear(PlotControl);
            IsZoomingRectangle = false;
        }

        // this covers the case where an extremely tiny zoom rectangle was made
        if ((isDragging == false) && (button == Inputs.ClickAutoAxisButton))
        {
            Actions.AutoScale(PlotControl, position);
        }

        if (IsZoomingRectangle && button == Inputs.DragZoomRectangleButton)
        {
            Actions.ZoomRectangleClear(PlotControl);
            IsZoomingRectangle = false;
        }

        if (!isDragging && (button == Inputs.ClickContextMenuButton))
        {
            Actions.ShowContextMenu(PlotControl, position);
        }

        Mouse.Up(button);
    }

    public virtual void DoubleClick()
    {
        Actions.ToggleBenchmark(PlotControl);
    }

    public virtual void MouseWheelVertical(Pixel pixel, float delta)
    {
        if (IsZoomingRectangle) return;

        MouseWheelDirection direction = delta > 0 ? MouseWheelDirection.Up : MouseWheelDirection.Down;

        if (Inputs.ZoomInWheelDirection.HasValue && Inputs.ZoomInWheelDirection == direction)
        {
            Actions.ZoomIn(PlotControl, pixel, new LockedAxes(LockX, LockY));
        }
        else if (Inputs.ZoomOutWheelDirection.HasValue && Inputs.ZoomOutWheelDirection == direction)
        {
            Actions.ZoomOut(PlotControl, pixel, new LockedAxes(LockX, LockY));
        }
        else if (Inputs.PanUpWheelDirection.HasValue && Inputs.PanUpWheelDirection == direction)
        {
            Actions.PanUp(PlotControl);
        }
        else if (Inputs.PanDownWheelDirection.HasValue && Inputs.PanDownWheelDirection == direction)
        {
            Actions.PanDown(PlotControl);
        }
        else if (Inputs.PanRightWheelDirection.HasValue && Inputs.PanRightWheelDirection == direction)
        {
            Actions.PanRight(PlotControl);
        }
        else if (Inputs.PanLeftWheelDirection.HasValue && Inputs.PanLeftWheelDirection == direction)
        {
            Actions.PanLeft(PlotControl);
        }
        else
        {
            return;
        }
    }
}
