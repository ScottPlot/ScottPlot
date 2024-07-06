namespace ScottPlot.Interactivity;

public class UserInputQueue
{
    public readonly List<IUserInput> Events = [];

    public void Add(IUserInput inputEvent)
    {
        Events.Add(inputEvent);
    }

    public void Clear()
    {
        Events.Clear();
    }
}
