namespace ScottPlot.Interactivity.UserInputActions;

public class RightClickDragZoom : IUserInputAction
{
    public void Reset()
    {
    }

    public UserActionResult Execute(Plot plot, IUserInput userInput)
    {
        return UserActionResult.NotRelevant();
    }
}
