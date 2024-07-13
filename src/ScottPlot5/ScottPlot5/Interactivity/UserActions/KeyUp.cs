namespace ScottPlot.Interactivity.UserInputs;

public record struct KeyUp(Key Key) : IUserAction
{
    public DateTime DateTime { get; set; } = DateTime.Now;
}
