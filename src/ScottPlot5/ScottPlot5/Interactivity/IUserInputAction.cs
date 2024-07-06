namespace ScottPlot.Interactivity;

public interface IUserInputAction
{
    UserActionResult Execute(Plot plot, UserInputQueue queue);
}
