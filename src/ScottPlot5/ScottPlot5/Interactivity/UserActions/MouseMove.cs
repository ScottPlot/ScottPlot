namespace ScottPlot.Interactivity.UserInputs;

public record struct MouseMove(Pixel Pixel) : IMouseInput
{
    public DateTime DateTime { get; set; } = DateTime.Now;
}
