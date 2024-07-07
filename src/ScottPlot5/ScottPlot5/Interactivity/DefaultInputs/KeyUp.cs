namespace ScottPlot.Interactivity.DefaultInputs;

public record struct KeyUp(string Name) : IUserInput
{
    public DateTime DateTime { get; set; } = DateTime.Now;
}
