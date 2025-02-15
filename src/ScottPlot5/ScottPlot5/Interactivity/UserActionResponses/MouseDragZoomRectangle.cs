namespace ScottPlot.Interactivity.UserActionResponses;

/// <summary>
/// Click-drag draws a rectangle over a plot which becomes the new field of view when released.
/// </summary>
public class MouseDragZoomRectangle(MouseButton button) : IUserActionResponse
{
    Pixel MouseDownPixel = Pixel.NaN;

    Plot? PlotBeingDragged = null;

    /// <summary>
    /// A zoom rectangle is started when this button is pressed and dragged
    /// </summary>
    MouseButton MouseButton { get; set; } = button;

    /// <summary>
    /// A zoom rectangle is started when this button is pressed and dragged with <see cref="SecondaryKey"/> held down
    /// </summary>
    public MouseButton SecondaryMouseButton { get; set; } = StandardMouseButtons.Left;

    /// <summary>
    /// A zoom rectangle is started when <see cref="SecondaryMouseButton"/> is pressed and dragged with this key held down
    /// </summary>
    public Key SecondaryKey { get; set; } = StandardKeys.Alt;

    /// <summary>
    /// When held, horizontal axis limits will not be modified
    /// </summary>
    public Key HorizontalLockKey { get; set; } = StandardKeys.Control;

    /// <summary>
    /// When held, vertical axis limits will not be modified
    /// </summary>
    public Key VerticalLockKey { get; set; } = StandardKeys.Shift;

    public void ResetState(IPlotControl plotControl)
    {
        MouseDownPixel = Pixel.NaN;

        if (PlotBeingDragged is not null)
        {
            PlotBeingDragged.ZoomRectangle.IsVisible = false;
        }

        PlotBeingDragged = null;
    }

    public ResponseInfo Execute(IPlotControl plotControl, IUserAction userAction, KeyboardState keys)
    {
        if (userAction is IMouseButtonAction buttonAction && buttonAction.IsPressed)
        {
            bool isPrimaryButton = buttonAction.Button == MouseButton;
            bool isSecondaryButtonAndKey = buttonAction.Button == SecondaryMouseButton && keys.IsPressed(SecondaryKey);
            if (isPrimaryButton || isSecondaryButtonAndKey)
            {
                Plot? plotUnderMouse = plotControl.GetPlotAtPixel(buttonAction.Pixel);
                if (plotUnderMouse is not null)
                {
                    MouseDownPixel = buttonAction.Pixel;
                    PlotBeingDragged = plotUnderMouse;
                    return new ResponseInfo() { IsPrimary = isSecondaryButtonAndKey };
                }
            }
        }

        if (MouseDownPixel == Pixel.NaN)
        {
            return ResponseInfo.NoActionRequired;
        }

        if (PlotBeingDragged is null)
        {
            return ResponseInfo.NoActionRequired;
        }

        if (userAction is IMouseAction mouseMoveAction && userAction is not IMouseButtonAction)
        {
            double dX = Math.Abs(MouseDownPixel.X - mouseMoveAction.Pixel.X);
            double dY = Math.Abs(MouseDownPixel.Y - mouseMoveAction.Pixel.Y);

            if (dX < 5 && dY < 5)
            {
                bool zoomRectWasPreviouslyVisible = PlotBeingDragged.ZoomRectangle.IsVisible;
                PlotBeingDragged.ZoomRectangle.IsVisible = false;

                return new ResponseInfo()
                {
                    RefreshNeeded = zoomRectWasPreviouslyVisible,
                    IsPrimary = false,
                };
            }

            PlotBeingDragged.ZoomRectangle.IsVisible = true;
            PlotBeingDragged.ZoomRectangle.HorizontalSpan = keys.IsPressed(VerticalLockKey);
            PlotBeingDragged.ZoomRectangle.VerticalSpan = keys.IsPressed(HorizontalLockKey);
            MouseAxisManipulation.PlaceZoomRectangle(PlotBeingDragged, MouseDownPixel, mouseMoveAction.Pixel);

            return new ResponseInfo() { RefreshNeeded = true, IsPrimary = true };
        }

        if (userAction is IMouseButtonAction mouseUpAction && !mouseUpAction.IsPressed)
        {
            MouseDownPixel = Pixel.NaN;

            if (PlotBeingDragged.ZoomRectangle.IsVisible)
            {
                PlotBeingDragged.Axes.GetAxes().OfType<IXAxis>().ToList().ForEach(PlotBeingDragged.ZoomRectangle.Apply);
                PlotBeingDragged.Axes.GetAxes().OfType<IYAxis>().ToList().ForEach(PlotBeingDragged.ZoomRectangle.Apply);
                PlotBeingDragged.ZoomRectangle.IsVisible = false;
                return ResponseInfo.Refresh;
            }
        }

        return ResponseInfo.NoActionRequired;
    }
}
