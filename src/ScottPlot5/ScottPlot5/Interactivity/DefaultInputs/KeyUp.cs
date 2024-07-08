namespace ScottPlot.Interactivity.DefaultInputs;

public record struct KeyUp(IKey Key) : IUserInput
{
    public DateTime DateTime { get; set; } = DateTime.Now;
}
