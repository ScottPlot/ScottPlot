namespace ScottPlot.Interactivity.UserActions;

public record struct MouseMove(Pixel Pixel) : IMouseAction
{
    public readonly string Device => $"mouse";
    public readonly string Description => $"mouse moved to {Pixel}";
    public DateTime DateTime { get; set; } = DateTime.Now;
}
