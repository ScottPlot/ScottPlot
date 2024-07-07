namespace ScottPlot.Interactivity;

/// <summary>
/// This class collects user inputs and performs actions to manipulate a Plot.
/// Custom user inputs may be supplied, and the list of responsive actions can be 
/// modified to achieve extreme control over interaction behavior.
/// </summary>
public class UserInputProcessor
{
    public readonly Plot Plot;
    public readonly List<IUserInputAction> InputActions;

    public UserInputProcessor(Plot plot)
    {
        Plot = plot;

        InputActions = [
            new UserInputActions.LeftClickDragPan(),
            new UserInputActions.RightClickDragZoom(),
        ];

        ResetAllActions();
    }

    public UserActionResult[] Add(IUserInput inputEvent)
    {
        List<UserActionResult> notableResults = [];

        bool refreshRequired = false;

        foreach (IUserInputAction action in InputActions)
        {
            UserActionResult result = action.Execute(Plot, inputEvent);

            if (result == UserActionResult.NotRelevant())
                continue;

            notableResults.Add(result);

            if (result.RefreshRequired)
                refreshRequired = true;

            if (result.ResetAllActions)
            {
                ResetAllActions();
                break;
            }
        }

        if (refreshRequired)
            Plot.PlotControl?.Refresh();

        return notableResults.ToArray();
    }

    private void ResetAllActions()
    {
        InputActions.ForEach(x => x.Reset());
    }
}
