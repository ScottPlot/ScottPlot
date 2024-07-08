namespace ScottPlot.Interactivity.UserInputResponses;

public class MiddleClickAutoscale : IUserInputResponse
{
    private Pixel MouseDownPixel = Pixel.NaN;

    public UserInputResponseResult Execute(Plot plot, IUserInput userInput, KeyState keys)
    {
        if (userInput is DefaultInputs.MiddleMouseDown mouseDownInput)
        {
            MouseDownPixel = mouseDownInput.Pixel;
            return new UserInputResponseResult()
            {
                Summary = $"middle click AutoScale STARTED at {MouseDownPixel}",
            };
        }

        if (MouseDownPixel == Pixel.NaN)
            return UserInputResponseResult.NoActionTaken;

        if (userInput is DefaultInputs.MiddleMouseUp mouseUpInput)
        {
            double dX = Math.Abs(MouseDownPixel.X - mouseUpInput.Pixel.X);
            double dY = Math.Abs(MouseDownPixel.Y - mouseUpInput.Pixel.Y);
            double dragDistance = Math.Sqrt(dX * dX + dY * dY);

            MouseDownPixel = Pixel.NaN;

            if (dragDistance > 5)
            {
                return new UserInputResponseResult()
                {
                    Summary = $"middle click AutoScale ABORTED because drag distance was large ({dragDistance:N2} px)",
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

