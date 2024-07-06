namespace ScottPlot.Interactivity.DefaultInputs;

public struct LeftMouseUp(Pixel pixel) : IUserInput
{
    public DateTime DateTime { get; set; } = DateTime.Now;
    public Pixel Pixel { get; set; } = pixel;
}
