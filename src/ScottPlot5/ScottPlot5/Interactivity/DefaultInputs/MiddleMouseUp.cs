namespace ScottPlot.Interactivity.DefaultInputs;

public record struct MiddleMouseUp(Pixel Pixel) : IUserInput
{
    public DateTime DateTime { get; set; } = DateTime.Now;
}
