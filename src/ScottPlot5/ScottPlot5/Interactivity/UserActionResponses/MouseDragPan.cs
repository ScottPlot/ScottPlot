namespace ScottPlot.Interactivity.UserActionResponses;

public class MouseDragPan(MouseButton button) : IUserActionResponse
{
    /// <summary>
    /// Which mouse button to watch for click-drag events
    /// </summary>
    public MouseButton MouseButton { get; } = button;

    private Pixel MouseDownPixel = Pixel.NaN;

    private MultiAxisLimits? RememberedLimits = null;

    /// <summary>
    /// Vertical panning is disabled if any of these keys are pressed
    /// </summary>
    public List<Key> KeysThatLockY { get; } = [StandardKeys.Control];

    /// <summary>
    /// Horizontal panning is disabled if any of these keys are pressed
    /// </summary>
    public List<Key> KeysThatLockX { get; } = [StandardKeys.Shift];

    /// <summary>
    /// Vertical panning is disabled when this is set
    /// </summary>
    public bool LockY { get; set; } = false;

    /// <summary>
    /// Horizontal panning is disabled when this is set
    /// </summary>
    public bool LockX { get; set; } = false;

    /// <summary>
    /// If enabled, mouse interactions over a single axis will be applied to all axes with the same orientation.
    /// </summary>
    public bool ChangeOpposingAxesTogether { get; set; } = false;

    public void ResetState(IPlotControl plotControl)
    {
        RememberedLimits = null;
        MouseDownPixel = Pixel.NaN;
    }

    public ResponseInfo Execute(IPlotControl plotControl, IUserAction userInput, KeyboardState keys)
    {
        // mouse down starts drag
        if (userInput is IMouseButtonAction mouseDownAction
            && mouseDownAction.Button == MouseButton
            && mouseDownAction.IsPressed)
        {
            Plot? plot = plotControl.GetPlotAtPixel(mouseDownAction.Pixel);
            if (plot is not null)
            {
                RememberedLimits = new(plot);
                MouseDownPixel = mouseDownAction.Pixel;
            }

            return ResponseInfo.NoActionRequired;
        }

        // mouse up ends drag
        if (userInput is IMouseButtonAction mouseUpAction
            && mouseUpAction.Button == MouseButton
            && !mouseUpAction.IsPressed
            && RememberedLimits is not null)
        {
            RememberedLimits.Recall();
            Plot plot = RememberedLimits.Plot;
            RememberedLimits = null;
            ApplyToPlot(plot, MouseDownPixel, mouseUpAction.Pixel, keys);

            return ResponseInfo.Refresh;
        }

        // mouse move while dragging
        if (userInput is IMouseAction mouseAction
            && RememberedLimits is not null)
        {
            // Ignore dragging if it's only a few pixels. This leaves room for
            // single- and double-click events that may drag by a few pixels accidentally.
            double dX = Math.Abs(mouseAction.Pixel.X - MouseDownPixel.X);
            double dY = Math.Abs(mouseAction.Pixel.Y - MouseDownPixel.Y);
            double maxDragDistance = Math.Max(dX, dY);
            if (maxDragDistance < 5)
            {
                return ResponseInfo.NoActionRequired;
            }

            RememberedLimits.Recall();
            Plot plot = RememberedLimits.Plot;
            ApplyToPlot(plot, MouseDownPixel, mouseAction.Pixel, keys);
            return new ResponseInfo() { RefreshNeeded = true, IsPrimary = true };
        }

        return ResponseInfo.NoActionRequired;
    }

    private void ApplyToPlot(Plot plot, Pixel px1, Pixel px2, KeyboardState keys)
    {
        if (LockX || keys.IsPressed(KeysThatLockX))
        {
            px2.X = px1.X;
        }

        if (LockY || keys.IsPressed(KeysThatLockY))
        {
            px2.Y = px1.Y;
        }

        MouseAxisManipulation.DragPan(plot, px1, px2, ChangeOpposingAxesTogether);
    }
}
