namespace ScottPlot.Interactivity.DefaultInputs;

public record struct LeftMouseDown(Pixel Pixel) : IUserInput
{
    public DateTime DateTime { get; set; } = DateTime.Now;
}
