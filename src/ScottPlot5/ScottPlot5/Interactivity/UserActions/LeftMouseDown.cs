namespace ScottPlot.Interactivity.UserInputs;

public record struct LeftMouseDown(Pixel Pixel) : IMouseInput
{
    public DateTime DateTime { get; set; } = DateTime.Now;
}
