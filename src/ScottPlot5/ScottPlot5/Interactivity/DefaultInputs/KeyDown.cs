namespace ScottPlot.Interactivity.DefaultInputs;

public record struct KeyDown(string Name) : IUserInput
{
    public DateTime DateTime { get; set; } = DateTime.Now;
}
