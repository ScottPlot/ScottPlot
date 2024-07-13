namespace ScottPlot.Interactivity.UserInputs;

public record struct MouseWheelUp(Pixel Pixel) : IMouseInput
{
    public DateTime DateTime { get; set; } = DateTime.Now;
}
