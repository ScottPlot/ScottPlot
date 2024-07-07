namespace ScottPlot.Interactivity.DefaultInputs;

public record struct RightMouseUp(Pixel Pixel) : IUserInput
{
    public DateTime DateTime { get; set; } = DateTime.Now;
}
