namespace ScottPlot.Interactivity.UserActionResponses;

public class SingleClickResponse(MouseButton button, Action<IPlotControl, Pixel> action) : IUserActionResponse
{
    /// <summary>
    /// Which mouse button to watch for single-click events
    /// </summary>
    public MouseButton MouseButton { get; } = button;

    /// <summary>
    /// This action is invoked when a single-click occurs.
    /// </summary>
    public Action<IPlotControl, Pixel> ResponseAction { get; } = action;

    /// <summary>
    /// Location of the previous mouse down event
    /// </summary>
    private Pixel MouseDownPixel = Pixel.NaN;

    public void ResetState(IPlotControl plotControl)
    {
        MouseDownPixel = Pixel.NaN;
    }

    public ResponseInfo Execute(IPlotControl plotControl, IUserAction userAction, KeyboardState keys)
    {
        if (userAction is IMouseButtonAction buttonAction && buttonAction.Button == MouseButton)
        {
            Plot? plot = plotControl.GetPlotAtPixel(buttonAction.Pixel);
            if (plot is null)
            {
                return ResponseInfo.NoActionRequired;
            }

            if (buttonAction.IsPressed)
            {
                MouseDownPixel = buttonAction.Pixel;
                return ResponseInfo.NoActionRequired;
            }
            else
            {
                if (double.IsNaN(MouseDownPixel.X))
                    return ResponseInfo.NoActionRequired;

                // do not respond to mouse dragging
                double dX = Math.Abs(MouseDownPixel.X - buttonAction.Pixel.X);
                double dY = Math.Abs(MouseDownPixel.Y - buttonAction.Pixel.Y);
                double rightClickDragDistance = Math.Max(dX, dY);
                if (rightClickDragDistance >= 5)
                {
                    MouseDownPixel = Pixel.NaN;
                    return ResponseInfo.NoActionRequired;
                }

                ResponseAction.Invoke(plotControl, buttonAction.Pixel);
                MouseDownPixel = Pixel.NaN;

                return new ResponseInfo() { RefreshNeeded = true };
            }
        }

        return ResponseInfo.NoActionRequired;
    }
}
