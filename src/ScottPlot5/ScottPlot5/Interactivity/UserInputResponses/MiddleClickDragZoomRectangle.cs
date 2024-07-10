using ScottPlot.Interactivity.UserInputs;

namespace ScottPlot.Interactivity.UserInputResponses;

public class MiddleClickDragZoomRectangle : IUserInputResponse
{
    private Pixel MouseDownPixel = Pixel.NaN;

    public UserInputResponseResult Execute(Plot plot, IUserInput userInput, KeyState keys)
    {
        if (userInput is UserInputs.MiddleMouseDown mouseDownInput)
        {
            MouseDownPixel = mouseDownInput.Pixel;
            return new UserInputResponseResult()
            {
                Summary = $"middle-click-drag zoom rectangle STARTED at {MouseDownPixel}",
            };
        }

        if (userInput is UserInputs.LeftMouseDown leftMouseDownInput && keys.IsPressed(StandardKeys.Alt))
        {
            MouseDownPixel = leftMouseDownInput.Pixel;
            return new UserInputResponseResult()
            {
                Summary = $"ALT + left-click-drag zoom rectangle STARTED at {MouseDownPixel}",
            };
        }

        if (MouseDownPixel == Pixel.NaN)
        {
            return UserInputResponseResult.NoActionTaken;
        }

        if (userInput is UserInputs.MouseMove mouseMoveInput)
        {
            double dX = Math.Abs(MouseDownPixel.X - mouseMoveInput.Pixel.X);
            double dY = Math.Abs(MouseDownPixel.Y - mouseMoveInput.Pixel.Y);
            double dragDistance = Math.Sqrt(dX * dX + dY * dY);

            if (dragDistance <= 10)
            {
                plot.ZoomRectangle.IsVisible = false;
                return new UserInputResponseResult()
                {
                    Summary = $"middle-click-drag zoom rectangle IGNORED because drag distance was too small ({dragDistance:N2} px)",
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
                IsPrimaryDragResponse = true,
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
                    IsPrimaryDragResponse = false,
                };
            }
        }

        return new UserInputResponseResult()
        {
            Summary = $"middle-click-drag zoom rectangle ignored {userInput}",
            IsPrimaryDragResponse = plot.ZoomRectangle.IsVisible,
        };
    }
}
