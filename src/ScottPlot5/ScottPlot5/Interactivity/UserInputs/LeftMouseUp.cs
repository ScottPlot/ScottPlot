namespace ScottPlot.Interactivity.UserInputs;

public record struct LeftMouseUp(Pixel Pixel) : IUserInput
{
    public DateTime DateTime { get; set; } = DateTime.Now;
}
