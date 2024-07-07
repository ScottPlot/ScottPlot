namespace ScottPlot.Interactivity.DefaultInputs;

public record struct MouseWheelUp(Pixel Pixel) : IUserInput
{
    public DateTime DateTime { get; set; } = DateTime.Now;
}
