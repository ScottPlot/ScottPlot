namespace ScottPlot.Interactivity.PlotResponses;

/// <summary>
/// Click-drag draws a rectangle over a plot which becomes the new field of view when released.
/// </summary>
public class MouseDragZoomRectangle(MouseButton button) : IPlotResponse
{
    Pixel MouseDownPixel = Pixel.NaN;

    /// <summary>
    /// A zoom rectangle is started when this button is pressed and dragged
    /// </summary>
    MouseButton MouseButton { get; set; } = button;

    /// <summary>
    /// A zoom rectangle is started when this button is pressed and dragged with <paramref name="SecondaryKey"/>held down
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

    public PlotResponseResult Execute(Plot plot, IUserAction userAction, KeyboardState keys)
    {
        if (userAction is IMouseButtonAction buttonAction && buttonAction.IsPressed)
        {
            bool isPrimaryButton = buttonAction.Button == MouseButton;
            bool isSecondaryButtonAndKey = buttonAction.Button == SecondaryMouseButton && keys.IsPressed(SecondaryKey);
            if (isPrimaryButton || isSecondaryButtonAndKey)
            {
                MouseDownPixel = buttonAction.Pixel;
                return new PlotResponseResult()
                {
                    Summary = $"ZoomRectangle STARTED {MouseDownPixel}",
                    IsPrimaryResponse = isSecondaryButtonAndKey,
                };
            }
        }

        if (MouseDownPixel == Pixel.NaN)
        {
            return PlotResponseResult.NoActionTaken;
        }

        if (userAction is IMouseAction mouseMoveAction && userAction is not IMouseButtonAction)
        {
            double dX = Math.Abs(MouseDownPixel.X - mouseMoveAction.Pixel.X);
            double dY = Math.Abs(MouseDownPixel.Y - mouseMoveAction.Pixel.Y);

            if (dX < 5 && dY < 5)
            {
                bool zoomRectWasPreviouslyVisible = plot.ZoomRectangle.IsVisible;
                plot.ZoomRectangle.IsVisible = false;
                return new PlotResponseResult()
                {
                    Summary = $"ZoomRectangle ignored small distance",
                    RefreshRequired = zoomRectWasPreviouslyVisible,
                    IsPrimaryResponse = false,
                };
            }

            plot.ZoomRectangle.IsVisible = true;
            plot.ZoomRectangle.MouseDown = MouseDownPixel;
            plot.ZoomRectangle.MouseUp = mouseMoveAction.Pixel;
            plot.ZoomRectangle.HorizontalSpan = keys.IsPressed(VerticalLockKey);
            plot.ZoomRectangle.VerticalSpan = keys.IsPressed(HorizontalLockKey);

            return new PlotResponseResult()
            {
                Summary = $"ZoomRectangle Updated",
                RefreshRequired = true,
                IsPrimaryResponse = true,
            };
        }

        if (userAction is IMouseButtonAction mouseUpAction && !mouseUpAction.IsPressed)
        {
            MouseDownPixel = Pixel.NaN;

            if (plot.ZoomRectangle.IsVisible)
            {
                plot.Axes.GetAxes().OfType<IXAxis>().ToList().ForEach(plot.ZoomRectangle.Apply);
                plot.Axes.GetAxes().OfType<IYAxis>().ToList().ForEach(plot.ZoomRectangle.Apply);

                plot.ZoomRectangle.IsVisible = false;

                return new PlotResponseResult()
                {
                    Summary = $"ZoomRectangle APPLIED",
                    RefreshRequired = true,
                    IsPrimaryResponse = false,
                };
            }
        }

        return PlotResponseResult.NoActionTaken;
    }
}
