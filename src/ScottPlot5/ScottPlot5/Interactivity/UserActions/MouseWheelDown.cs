namespace ScottPlot.Interactivity.UserInputs;

public record struct MouseWheelDown(Pixel Pixel) : IMouseInput
{
    public DateTime DateTime { get; set; } = DateTime.Now;
}
