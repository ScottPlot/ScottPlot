namespace ScottPlot.Interactivity.UserInputs;

public record struct KeyDown(IKey Key) : IUserInput
{
    public DateTime DateTime { get; set; } = DateTime.Now;
}
