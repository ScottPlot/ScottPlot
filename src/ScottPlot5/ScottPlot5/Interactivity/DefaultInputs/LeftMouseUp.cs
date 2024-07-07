namespace ScottPlot.Interactivity.DefaultInputs;

public record struct LeftMouseUp(Pixel Pixel) : IUserInput
{
    public DateTime DateTime { get; set; } = DateTime.Now;
}
