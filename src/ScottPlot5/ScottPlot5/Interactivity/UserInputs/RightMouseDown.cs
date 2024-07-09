namespace ScottPlot.Interactivity.UserInputs;

public record struct RightMouseDown(Pixel Pixel) : IUserInput
{
    public DateTime DateTime { get; set; } = DateTime.Now;
}
