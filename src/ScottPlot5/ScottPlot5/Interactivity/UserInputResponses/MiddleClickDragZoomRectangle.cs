namespace ScottPlot.Interactivity.UserInputResponses;

public class MiddleClickDragZoomRectangle : IUserInputResponse
{
    private Pixel MouseDownPixel = Pixel.NaN;

    public void Reset()
    {
    }

    public UserInputResponseResult Execute(Plot plot, IUserInput userInput, KeyState keys)
    {
        if (userInput is UserInputs.MiddleMouseDown mouseDownInput)
        {
            MouseDownPixel = mouseDownInput.Pixel;
            return new UserInputResponseResult()
            {
                Summary = $"middle click zoom rectangle STARTED at {MouseDownPixel}",
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
                    Summary = $"middle click zoom rectangle IGNORED because drag distance was too small ({dragDistance:N2} px)",
                };
            }

            plot.ZoomRectangle.IsVisible = true;
            plot.ZoomRectangle.MouseDown = MouseDownPixel;
            plot.ZoomRectangle.MouseUp = mouseMoveInput.Pixel;

            return new UserInputResponseResult()
            {
                Summary = $"middle click zoom rectangle UPDATED",
                RefreshRequired = true,
                IsPrimaryResponse = true,
            };
        }

        if (userInput is UserInputs.MiddleMouseUp mouseUpInput)
        {
            MouseDownPixel = Pixel.NaN;
            if (plot.ZoomRectangle.IsVisible)
            {
                plot.Axes.GetAxes().OfType<IXAxis>().ToList().ForEach(plot.ZoomRectangle.Apply);
                plot.Axes.GetAxes().OfType<IYAxis>().ToList().ForEach(plot.ZoomRectangle.Apply);

                plot.ZoomRectangle.IsVisible = false;

                return new UserInputResponseResult()
                {
                    Summary = $"middle click zoom rectangle APPLIED",
                    RefreshRequired = true,
                    IsPrimaryResponse = false,
                };
            }
        }

        return new UserInputResponseResult()
        {
            Summary = $"middle click zoom rectangle ignored {userInput}",
            IsPrimaryResponse = plot.ZoomRectangle.IsVisible,
        };
    }
}
