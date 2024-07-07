namespace ScottPlot.Interactivity.DefaultInputs;

public record struct MouseWheelDown(Pixel Pixel) : IUserInput
{
    public DateTime DateTime { get; set; } = DateTime.Now;
}
