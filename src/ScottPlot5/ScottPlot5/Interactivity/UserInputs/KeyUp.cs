namespace ScottPlot.Interactivity.UserInputs;

public record struct KeyUp(IKey Key) : IUserInput
{
    public DateTime DateTime { get; set; } = DateTime.Now;
}
