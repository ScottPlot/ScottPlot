namespace ScottPlot.Interactivity;

public interface IUserInputAction
{
    UserActionResult Execute(Plot plot, IUserInput userInput);
    void Reset();
}
