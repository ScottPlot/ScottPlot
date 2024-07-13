namespace ScottPlot.Interactivity.UserInputs;

public record struct RightMouseUp(Pixel Pixel) : IMouseInput
{
    public DateTime DateTime { get; set; } = DateTime.Now;
}
