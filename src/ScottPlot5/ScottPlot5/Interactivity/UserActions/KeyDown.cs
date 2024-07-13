namespace ScottPlot.Interactivity.UserInputs;

public record struct KeyDown(Key Key) : IUserAction
{
    public DateTime DateTime { get; set; } = DateTime.Now;
}
