namespace ScottPlot.Interactivity.UserInputResponses;

public class MiddleClickAutoscale : IUserInputResponse
{
    private Pixel MouseDownPixel = Pixel.NaN;

    public UserInputResponseResult Execute(Plot plot, IUserInput userInput, KeyState keys)
    {
        if (userInput is UserInputs.MiddleMouseDown mouseDownInput)
        {
            MouseDownPixel = mouseDownInput.Pixel;
            return new UserInputResponseResult()
            {
                Summary = $"middle click AutoScale STARTED at {MouseDownPixel}",
            };
        }

        if (MouseDownPixel == Pixel.NaN)
            return UserInputResponseResult.NoActionTaken;

        if (userInput is UserInputs.MiddleMouseUp mouseUpInput)
        {
            double dX = Math.Abs(MouseDownPixel.X - mouseUpInput.Pixel.X);
            double dY = Math.Abs(MouseDownPixel.Y - mouseUpInput.Pixel.Y);

            MouseDownPixel = Pixel.NaN;

            if (dX > 5 || dY > 5)
            {
                return new UserInputResponseResult()
                {
                    Summary = $"middle click AutoScale ABORTED because drag distance was large",
                };
            }

            plot.Axes.AutoScale();
            return new UserInputResponseResult()
            {
                Summary = $"middle click AutoScale APPLIED",
                RefreshRequired = true,
            };
        }

        return UserInputResponseResult.NoActionTaken;
    }
}

