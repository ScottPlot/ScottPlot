using ScottPlot.Interactivity.UserInputs;

namespace ScottPlot.Interactivity.UserInputResponses;

public class MiddleClickDragZoomRectangle : IUserInputResponse
{
    private Pixel MouseDownPixel = Pixel.NaN;

    public UserInputResponseResult Execute(Plot plot, IUserInput userInput, KeyState keys)
    {
        if (userInput is MiddleMouseDown mouseDownInput)
        {
            MouseDownPixel = mouseDownInput.Pixel;
            return new UserInputResponseResult()
            {
                Summary = $"middle-click-drag zoom rectangle STARTED at {MouseDownPixel}",
                IsPrimaryResponse = false,
            };
        }

        if (userInput is LeftMouseDown leftMouseDownInput && keys.IsPressed(StandardKeys.Alt))
        {
            MouseDownPixel = leftMouseDownInput.Pixel;
            return new UserInputResponseResult()
            {
                Summary = $"ALT + left-click-drag zoom rectangle STARTED at {MouseDownPixel}",
                IsPrimaryResponse = true,
            };
        }

        if (MouseDownPixel == Pixel.NaN)
        {
            return UserInputResponseResult.NoActionTaken;
        }

        if (userInput is MouseMove mouseMoveInput)
        {
            double dX = Math.Abs(MouseDownPixel.X - mouseMoveInput.Pixel.X);
            double dY = Math.Abs(MouseDownPixel.Y - mouseMoveInput.Pixel.Y);

            if (dX < 5 && dY < 5)
            {
                bool zoomRectWasPreviouslyVisible = plot.ZoomRectangle.IsVisible;
                plot.ZoomRectangle.IsVisible = false;
                return new UserInputResponseResult()
                {
                    Summary = $"middle-click-drag zoom rectangle IGNORED because drag distance was too small",
                    RefreshRequired = zoomRectWasPreviouslyVisible,
                    IsPrimaryResponse = false,
                };
            }

            plot.ZoomRectangle.IsVisible = true;
            plot.ZoomRectangle.MouseDown = MouseDownPixel;
            plot.ZoomRectangle.MouseUp = mouseMoveInput.Pixel;
            plot.ZoomRectangle.HorizontalSpan = keys.IsPressed(StandardKeys.Control);
            plot.ZoomRectangle.VerticalSpan = keys.IsPressed(StandardKeys.Shift);

            return new UserInputResponseResult()
            {
                Summary = $"middle-click-drag zoom rectangle UPDATED",
                RefreshRequired = true,
                IsPrimaryResponse = true,
            };
        }

        if (userInput is MiddleMouseUp || userInput is LeftMouseUp)
        {
            MouseDownPixel = Pixel.NaN;
            if (plot.ZoomRectangle.IsVisible)
            {
                plot.Axes.GetAxes().OfType<IXAxis>().ToList().ForEach(plot.ZoomRectangle.Apply);
                plot.Axes.GetAxes().OfType<IYAxis>().ToList().ForEach(plot.ZoomRectangle.Apply);

                plot.ZoomRectangle.IsVisible = false;

                return new UserInputResponseResult()
                {
                    Summary = $"middle-click-drag zoom rectangle APPLIED",
                    RefreshRequired = true,
                    IsPrimaryResponse = false,
                };
            }
        }

        return new UserInputResponseResult()
        {
            Summary = $"middle-click-drag zoom rectangle ignored {userInput}",
            IsPrimaryResponse = plot.ZoomRectangle.IsVisible,
        };
    }
}
