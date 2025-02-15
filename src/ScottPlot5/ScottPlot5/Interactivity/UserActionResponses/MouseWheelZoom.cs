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

    public void ResetState(IPlotControl plotControl) { }

    /// <summary>
    /// Fraction of the axis range to change when zooming in and out.
    /// </summary>
    public double ZoomFraction { get; set; } = 0.15;

    private double ZoomInFraction => 1 + ZoomFraction;
    private double ZoomOutFraction => 1 / ZoomInFraction;

    public ResponseInfo Execute(IPlotControl plotControl, IUserAction userInput, KeyboardState keys)
    {
        if (userInput is UserActions.MouseWheelUp mouseDownInput)
        {
            Plot? plot = plotControl.GetPlotAtPixel(mouseDownInput.Pixel);
            if (plot is null)
                return ResponseInfo.NoActionRequired;

            double xFrac = keys.IsPressed(LockHorizontalKey) ? 1 : ZoomInFraction;
            double yFrac = keys.IsPressed(LockVerticalKey) ? 1 : ZoomInFraction;
            MouseAxisManipulation.MouseWheelZoom(plot, xFrac, yFrac, mouseDownInput.Pixel, LockParallelAxes);
            return new ResponseInfo() { RefreshNeeded = true };
        }

        if (userInput is UserActions.MouseWheelDown mouseUpInput)
        {
            Plot? plot = plotControl.GetPlotAtPixel(mouseUpInput.Pixel);
            if (plot is null)
                return ResponseInfo.NoActionRequired;

            double xFrac = keys.IsPressed(LockHorizontalKey) ? 1 : ZoomOutFraction;
            double yFrac = keys.IsPressed(LockVerticalKey) ? 1 : ZoomOutFraction;
            MouseAxisManipulation.MouseWheelZoom(plot, xFrac, yFrac, mouseUpInput.Pixel, LockParallelAxes);
            return new ResponseInfo() { RefreshNeeded = true };
        }

        return ResponseInfo.NoActionRequired;
    }
}
