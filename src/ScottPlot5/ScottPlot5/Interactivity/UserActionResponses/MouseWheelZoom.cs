namespace ScottPlot.Interactivity.UserActionResponses;

public class MouseWheelZoom(Key horizontalLockKey, Key verticalLockKey) : IUserActionResponse
{
    /// <summary>
    /// If enabled, when the mouse zooms while hovered over an axis only that axis will be changed.
    /// </summary>
    public bool ZoomAxisUnderMouse { get; set; } = true;

    /// <summary>
    /// If enabled with <see cref="ZoomAxisUnderMouse"/>, all axes of the same direction will be changed together.
    /// </summary>
    public bool LockParallelAxes { get; set; } = false;

    Key LockHorizontalKey { get; } = horizontalLockKey;

    Key LockVerticalKey { get; } = verticalLockKey;

    public void ResetState(Plot plot) { }

    public ResponseInfo Execute(Plot plot, IUserAction userInput, KeyboardState keys)
    {
        if (userInput is UserActions.MouseWheelUp mouseDownInput)
        {
            double xFrac = keys.IsPressed(LockHorizontalKey) ? 1 : 1.15;
            double yFrac = keys.IsPressed(LockVerticalKey) ? 1 : 1.15;
            MouseAxisManipulation.MouseWheelZoom(plot, xFrac, yFrac, mouseDownInput.Pixel, LockParallelAxes);
            return new ResponseInfo() { RefreshNeeded = true };
        }

        if (userInput is UserActions.MouseWheelDown mouseUpInput)
        {
            double xFrac = keys.IsPressed(LockHorizontalKey) ? 1 : 0.85;
            double yFrac = keys.IsPressed(LockVerticalKey) ? 1 : 0.85;
            MouseAxisManipulation.MouseWheelZoom(plot, xFrac, yFrac, mouseUpInput.Pixel, LockParallelAxes);
            return new ResponseInfo() { RefreshNeeded = true };
        }

        return ResponseInfo.NoActionRequired;
    }
}
