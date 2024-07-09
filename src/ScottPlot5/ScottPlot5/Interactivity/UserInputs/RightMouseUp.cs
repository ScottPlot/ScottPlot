namespace ScottPlot.Interactivity.UserInputs;

public record struct RightMouseUp(Pixel Pixel) : IUserInput
{
    public DateTime DateTime { get; set; } = DateTime.Now;
}
