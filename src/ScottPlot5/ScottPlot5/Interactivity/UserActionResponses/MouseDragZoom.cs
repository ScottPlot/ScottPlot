namespace ScottPlot.Interactivity.UserActionResponses;

public class MouseDragZoom(MouseButton button) : IUserActionResponse
{
    Pixel MouseDownPixel = Pixel.NaN;

    MouseButton MouseButton { get; } = button;

    // TODO: re-implement this being more careful about allocations
    MultiAxisLimits? RememberedLimits = null;

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
    /// Scale the horizontal mouse sensitivity by this value.
    /// Larger numbers result in more zooming for the same drag distance.
    /// </summary>
    public double SensitivityX { get; set; } = 1.0;

    /// <summary>
    /// Scale the vertical mouse sensitivity by this value.
    /// Larger numbers result in more zooming for the same drag distance.
    /// </summary>
    public double SensitivityY { get; set; } = 1.0;

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
        if (userInput is IMouseButtonAction mouseDownAction && mouseDownAction.Button == MouseButton && mouseDownAction.IsPressed)
        {
            MouseDownPixel = mouseDownAction.Pixel;
            Plot? plot = plotControl.GetPlotAtPixel(mouseDownAction.Pixel);

            if (plot is not null)
            {
                RememberedLimits = new(plot);
                return new ResponseInfo() { IsPrimary = false };
            }
        }

        if (MouseDownPixel == Pixel.NaN)
            return ResponseInfo.NoActionRequired;

        if (userInput is IMouseButtonAction mouseUpAction && mouseUpAction.Button == MouseButton && !mouseUpAction.IsPressed)
        {
            if (RememberedLimits is not null)
            {
                RememberedLimits.Recall();
                ApplyToPlot(RememberedLimits.Plot, MouseDownPixel, mouseUpAction.Pixel, keys);
                MouseDownPixel = Pixel.NaN;
                return ResponseInfo.Refresh;
            }
        }

        if (userInput is IMouseAction mouseMoveAction)
        {
            if (RememberedLimits is not null)
            {
                RememberedLimits.Recall();
                ApplyToPlot(RememberedLimits.Plot, MouseDownPixel, mouseMoveAction.Pixel, keys);
                return new ResponseInfo() { RefreshNeeded = true, IsPrimary = true };
            }
        }

        return new ResponseInfo() { IsPrimary = true };
    }

    private void ApplyToPlot(Plot plot, Pixel px1, Pixel px2, KeyboardState keys)
    {
        float x1 = px1.X;
        float y1 = px1.Y;

        float x2 = px2.X;
        float y2 = px2.Y;

        if (LockX || keys.IsPressed(KeysThatLockX))
        {
            x2 = x1;
        }

        if (LockY || keys.IsPressed(KeysThatLockY))
        {
            y2 = y1;
        }

        if (SensitivityX != 1.0)
        {
            x2 = x1 + (x2 - x1) * (float)SensitivityX;
        }

        if (SensitivityY != 1.0)
        {
            y2 = y1 + (y2 - y1) * (float)SensitivityY;
        }

        px2 = new(x2, y2);

        MouseAxisManipulation.DragZoom(plot, px1, px2, ChangeOpposingAxesTogether);
    }
}
