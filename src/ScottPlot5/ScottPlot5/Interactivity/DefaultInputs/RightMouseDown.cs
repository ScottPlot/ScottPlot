namespace ScottPlot.Interactivity.DefaultInputs;

public record struct RightMouseDown(Pixel Pixel) : IUserInput
{
    public DateTime DateTime { get; set; } = DateTime.Now;
}
